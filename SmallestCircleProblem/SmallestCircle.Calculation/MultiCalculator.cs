﻿using SmallestCircle.Calculation.Geometry;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmallestCircle.Calculation
{

    public class MultiCalculator : CalculatorBase
    {
        protected IAsyncPointsIterator iterator;
        protected List<Point> points;
        protected int threadsCount;


        public MultiCalculator(IAsyncPointsIterator iterator, int threadsCount)
        {
            this.iterator = iterator;
            this.threadsCount = threadsCount;
            this.points = new List<Point>(iterator.PointsCount);
        }

        public virtual async Task<Circle> CalculateCircleAsync()
        {
            var firstPoints = (await iterator.GetManyAsync(2)).ToArray();
            var nextPointTask = iterator.GetNextAsync();

            var circle = CreateCircle.FromTwoPoints(firstPoints[0], firstPoints[1]);
            points.AddRange(firstPoints);

            var count = iterator.PointsCount;
            var nextPoint = await nextPointTask;

            while (nextPoint != null)
            {
                RaisePointProcessed(this, new OnPointDrawEventArgs(nextPoint));
                nextPointTask = iterator.GetNextAsync();

                if (!circle.ContainsPoint(nextPoint))
                {
                    // Update the circle to contain the new point as well:
                    circle = FindCircleCombination(nextPoint, points);
                }

                points.Add(nextPoint);
                nextPoint = await nextPointTask;
            }
            return circle;
        }

        protected Circle FindCircleCombination(Point newPoint, IList<Point> existingPoints)
        {
            var paralelOptions = new ParallelOptions { MaxDegreeOfParallelism = threadsCount };
            Circle minCircle = null;

            var currentThread = 1;


            Parallel.ForEach(existingPoints, paralelOptions, (otherPoint, loopstate) =>
            {
                var thread = System.Threading.Thread.CurrentThread.ManagedThreadId;
                var circle = CreateCircle.FromTwoPoints(newPoint, otherPoint);

                if (existingPoints.All(circle.ContainsPoint))
                {
                    if (minCircle == null || circle < minCircle)
                    {
                        minCircle = circle;
                        loopstate.Stop();
                    }
                }
            });           
            if (minCircle == null)
            {
                currentThread++;
               
                Parallel.For(0, existingPoints.Count, paralelOptions, (i, loopstate) =>
                {
                    for (int j = i + 1; j < existingPoints.Count; j++)
                    {
                        var circle = CreateCircle.FromThreePoints(newPoint, existingPoints[i], existingPoints[j]);

                        if (existingPoints.All(circle.ContainsPoint))
                        {
                            if (minCircle == null || circle < minCircle)
                            {
                                minCircle = circle;
                            }
                        }
                    }
                });             

            }

            currentThread++;

            return minCircle;
        }
    }
}
