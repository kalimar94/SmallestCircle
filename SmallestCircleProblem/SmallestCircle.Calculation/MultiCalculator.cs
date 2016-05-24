using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallestCircle.Calculation
{

    public class MultiCalculator
    {
        private IAsyncPointsIterator iterator;
        private List<Point> points;
        private int threadsCount;

        public MultiCalculator(IAsyncPointsIterator iterator, int threadsCount)
        {
            this.iterator = iterator;
            this.threadsCount = threadsCount;
            this.points = new List<Point>();
        }

        public async Task<Circle> CalculateCircle()
        {
            var firstPoints = (await iterator.GetManyAsync(2)).ToArray();
            var nextPointTask = iterator.GetNextAsync();

            var circle = Circle.FromTwoPoints(firstPoints[0], firstPoints[1]);
            points.AddRange(firstPoints);

            var count = iterator.PointsCount;
            var nextPoint = await nextPointTask;

            while (nextPoint != null)
            {
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

        private Circle FindCircleCombination(Point newPoint, IList<Point> existingPoints)
        {
            var paralelOptions = new ParallelOptions { MaxDegreeOfParallelism = threadsCount };
            Circle minCircle = null;

            Parallel.ForEach(existingPoints, paralelOptions, (otherPoint, loopstate) =>
            {
                var circle = Circle.FromTwoPoints(newPoint, otherPoint);

                if (existingPoints.All(circle.ContainsPoint))
                {
                    minCircle = circle;
                    loopstate.Stop();
                }
            });

            if (minCircle != null)
            {
                Parallel.For(0, existingPoints.Count, paralelOptions, (i, loopstate) =>
                {
                    for (int j = i + 1; j < existingPoints.Count; j++)
                    {
                        if (minCircle != null)
                            loopstate.Stop();

                        var circle = Circle.FromThreePoints(newPoint, existingPoints[i], existingPoints[j]);

                        if (existingPoints.All(circle.ContainsPoint))
                        {
                            minCircle = circle;
                            loopstate.Stop();
                        }
                    }
                }); 
            }

            return minCircle;
        }
    }
}
