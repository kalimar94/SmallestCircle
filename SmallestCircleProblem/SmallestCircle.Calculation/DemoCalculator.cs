using SmallestCircle.Calculation.Geometry;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
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

        public delegate void ThreadStartedHandler(object sender, OnThreadStartEventArgs e);
        public event ThreadStartedHandler OnThreadStarted;

        public delegate void ThreadStoppedHandler(object sender, OnThreadStopEventArgs e);
        public event ThreadStoppedHandler OnThreadStopped;


        private Circle circle;

        public List<Point> Points => this.points;

        public DemoCalculator(IAsyncPointsIterator iterator, int threadsCount, bool isQuiet)
         :base(iterator, threadsCount)
        {

        }

        public async Task<Circle> CalculateCircleAsync(CancellationToken token)
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
                    circle = FindCircleCombination(nextPoint, points);
                    OnCircleFound?.Invoke(this, new OnCircleDrawEventArgs(circle));
                }

                points.Add(nextPoint);
                nextPoint = await nextPointTask;
                await Task.Delay(300);
            }
            OnCircleFound?.Invoke(this, new OnCircleDrawEventArgs(circle));
            return circle;
        }

        protected Circle FindCircleCombination(Point newPoint, IList<Point> existingPoints)
        {
            var paralelOptions = new ParallelOptions { MaxDegreeOfParallelism = threadsCount };
            Circle minCircle = null;


            Parallel.ForEach(existingPoints, paralelOptions, (otherPoint, loopstate) =>
            {
                var thread = Thread.CurrentThread.ManagedThreadId;

                OnThreadStarted?.Invoke(this, new OnThreadStartEventArgs(thread));
                var circle = CreateCircle.FromTwoPoints(newPoint, otherPoint);

                if (circle.ContainsAllPoints(existingPoints))
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
                Parallel.For(0, existingPoints.Count, paralelOptions, (i, loopstate) =>
                {
                    var thread = Thread.CurrentThread.ManagedThreadId;
                    OnThreadStarted?.Invoke(this, new OnThreadStartEventArgs(thread));
                    for (int j = i + 1; j < existingPoints.Count; j++)
                    {
                        var circle = CreateCircle.FromThreePoints(newPoint, existingPoints[i], existingPoints[j]);

                        if (circle.ContainsAllPoints(existingPoints))
                        {
                            if (minCircle == null || circle < minCircle)
                            {
                                minCircle = circle;
                            }
                        }
                    }
                    OnThreadStopped?.Invoke(this, new OnThreadStopEventArgs(thread));
                });

            }
            return minCircle;
        }
    }
}
