using SmallestCircle.Calculation.Geometry;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmallestCircle.Calculation
{

    /// <summary> This calculator serves only for demonstration purposes - displaying the work of the algorithm </summary>
    public class DemoCalculator : MultiCalculator
    {
        public delegate void PointProssedHandler(object sender, OnPointDrawEventArgs e);
        public event PointProssedHandler OnPointProcessed;

        public delegate void CircleFoundHandler(object sender, OnCircleDrawEventArgs e);
        public event CircleFoundHandler OnCircleFound;

        public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);
        public event NotificationEventHandler OnNotification;


        private Circle circle;

        public List<Point> Points => this.points;

        public DemoCalculator(IAsyncPointsIterator iterator, int threadsCount)
         :base(iterator, threadsCount)
        {

        }

        public async Task<Circle> CalculateCircleAsync(CancellationToken token = default(CancellationToken))
        {
            var nextPointTask = iterator.GetNextAsync();
            if (points.Count < 2)
            {
                // Read the first two points:
                var firstPoints = (await iterator.GetManyAsync(2)).ToArray();
               
                circle = CreateCircle.FromTwoPoints(firstPoints[0], firstPoints[1]);

                OnPointProcessed?.Invoke(this, new OnPointDrawEventArgs(firstPoints[0]));
                OnPointProcessed?.Invoke(this, new OnPointDrawEventArgs(firstPoints[1]));
                OnCircleFound?.Invoke(this, new OnCircleDrawEventArgs(circle));
                points.AddRange(firstPoints);
            }

            var count = iterator.PointsCount;
            var nextPoint = await nextPointTask;

            while (nextPoint != null && !token.IsCancellationRequested)
            {
                OnPointProcessed?.Invoke(this, new OnPointDrawEventArgs(nextPoint));
                await Task.Delay(300);

                nextPointTask = iterator.GetNextAsync();

                if (!circle.ContainsPoint(nextPoint))
                {
                    // Update the circle to contain the new point as well:
                    circle = FindCircleCombination(nextPoint);
                    OnCircleFound?.Invoke(this, new OnCircleDrawEventArgs(circle));
                }

                points.Add(nextPoint);
                nextPoint = await nextPointTask;
                await Task.Delay(300);
            }
            OnCircleFound?.Invoke(this, new OnCircleDrawEventArgs(circle));
            return circle;
        }

        protected Circle FindCircleCombination(Point newPoint)
        {
            var paralelOptions = new ParallelOptions { MaxDegreeOfParallelism = threadsCount };
            Circle minCircle = null;

            Parallel.ForEach(points, paralelOptions, (otherPoint, loopstate) =>
            {
                var thread = Thread.CurrentThread.ManagedThreadId;
                OnNotification?.Invoke(this, $"Thread {thread} started, searching circle through 2 points");

                var circle = CreateCircle.FromTwoPoints(newPoint, otherPoint);

                if (circle.ContainsAllPoints(points))
                {
                    if (minCircle == null || circle < minCircle)
                    {
                        minCircle = circle;
                        loopstate.Stop();
                        OnNotification?.Invoke(this, $"Circle found, stopping all worker threads");
                    }
                }
            });

            if (minCircle == null)
            {
                Parallel.For(0, points.Count, paralelOptions, (i, loopstate) =>
                {
                    var thread = Thread.CurrentThread.ManagedThreadId;
                    OnNotification?.Invoke(this, $"Thread {thread} started, searching circle through 3 points");
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
                    OnNotification?.Invoke(this, $"Thread {thread} finished");
                });

            }

            OnNotification?.Invoke(this, $"Min circle found {circle}");
            return minCircle;
        }
    }
}
