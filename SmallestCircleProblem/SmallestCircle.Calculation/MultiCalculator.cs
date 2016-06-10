using SmallestCircle.Calculation.Geometry;
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
        public bool IsQuiet;


        public MultiCalculator(IAsyncPointsIterator iterator, int threadsCount, bool isQuiet)
        {
            this.iterator = iterator;
            this.threadsCount = threadsCount;
            this.points = new List<Point>(iterator.PointsCount);
            this.IsQuiet = isQuiet;
        }

        public virtual async Task<Circle> CalculateCircleAync()
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
                    RaiseCircleFound(this, new OnCircleDrawEventArgs(circle));
                }

                points.Add(nextPoint);
                nextPoint = await nextPointTask;
            }
            RaiseCircleFound(this, new OnCircleDrawEventArgs(circle));
            return circle;
        }

        protected Circle FindCircleCombination(Point newPoint, IList<Point> existingPoints)
        {
            var paralelOptions = new ParallelOptions { MaxDegreeOfParallelism = threadsCount };
            Circle minCircle = null;

            var currentThread = 1;

            if (!IsQuiet)
            {
                RaiseThreadStarted(this, new OnThreadStartEventArgs(currentThread));
            }

            Parallel.ForEach(existingPoints, paralelOptions, (otherPoint, loopstate) =>
            {
                var thread = System.Threading.Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"Starting thread {thread}: testing points {newPoint} and {otherPoint}");
                var circle = CreateCircle.FromTwoPoints(newPoint, otherPoint);

                if (existingPoints.All(circle.ContainsPoint))
                {
                    if (minCircle == null || circle < minCircle)
                    {
                        minCircle = circle;
                        Console.WriteLine($"Minimal circle found with {newPoint} and {otherPoint}. Terminating other threads");
                        loopstate.Stop();
                    }
                }
            });
            if (!IsQuiet)
            {
                RaiseThreadStopped(this, new OnThreadStopEventArgs(currentThread));
            }

            if (minCircle == null)
            {
                currentThread++;
                if (!IsQuiet)
                {
                    RaiseThreadStarted(this, new OnThreadStartEventArgs(currentThread));
                }

                Parallel.For(0, existingPoints.Count, paralelOptions, (i, loopstate) =>
                {
                    var thread = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"Starting thread {thread}: testing 3 points including {existingPoints[i]} and {newPoint}");

                    for (int j = i + 1; j < existingPoints.Count; j++)
                    {
                        var circle = CreateCircle.FromThreePoints(newPoint, existingPoints[i], existingPoints[j]);

                        if (existingPoints.All(circle.ContainsPoint))
                        {
                            if (minCircle == null || circle < minCircle)
                            {
                                minCircle = circle;
                                Console.WriteLine($"Found min circle through including {existingPoints[i]}, {existingPoints[j]} and {newPoint}");
                            }
                        }
                    }

                    Console.WriteLine($"Exit thread {thread}");
                });

                if (!IsQuiet)
                {
                    RaiseThreadStopped(this, new OnThreadStopEventArgs(currentThread));
                }

            }

            currentThread++;

            return minCircle;
        }
    }
}
