using SmallestCircle.Calculation.Geometry;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmallestCircle.Calculation
{

    public class MultiCalculator
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

        public async Task<Circle> CalculateCircleAsync()
        {
            var firstPoints = (await iterator.GetManyAsync(2)).ToArray();
            var nextPointTask = iterator.GetNextAsync();

            var circle = CreateCircle.FromTwoPoints(firstPoints[0].Value, firstPoints[1].Value);
            points.AddRange(firstPoints.Select(x=> x.Value));

            var count = iterator.PointsCount;
            var nextPoint = await nextPointTask;

            while (nextPoint != null)
            {
                nextPointTask = iterator.GetNextAsync();

                if (!circle.ContainsPoint(nextPoint.Value))
                {
                    // Update the circle to contain the new point as well:
                    circle = FindCircleCombination(nextPoint.Value);
                }

                points.Add(nextPoint.Value);
                nextPoint = await nextPointTask;
            }
            return circle;
        }

        private Circle FindCircleCombination(Point newPoint)
        {
            var paralelOptions = new ParallelOptions { MaxDegreeOfParallelism = threadsCount };
            Circle? minCircle = null;

            // Try all circles through two points - newPoint and one of the rest
            Parallel.ForEach(points, paralelOptions, (otherPoint, loopstate) =>
            {
                var circle = CreateCircle.FromTwoPoints(newPoint, otherPoint);

                if (circle.ContainsAllPoints(points))
                {
                    if (minCircle == null || circle < minCircle)
                    {
                        minCircle = circle;
                        loopstate.Stop();
                    }
                }
            });


            // If no circles are found yet, try all circles through nextPoint and two other points
            if (minCircle == null)
            {                              
                Parallel.For(0, points.Count, paralelOptions, (i, loopstate) =>
                {
                    for (int j = i + 1; j < points.Count; j++)
                    {
                        var circle = CreateCircle.FromThreePoints(newPoint, points[i], points[j]);

                        if (circle.ContainsAllPoints(points))
                        {
                            if (minCircle == null || circle < minCircle)
                            {
                                minCircle = circle;
                            }
                        }
                    }
                });             
            }

            return minCircle.Value;
        }
    }
}
