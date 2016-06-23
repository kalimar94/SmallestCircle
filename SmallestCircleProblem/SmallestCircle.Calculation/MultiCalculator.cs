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
        protected LinkedList<Point> points;
        protected int threadsCount;


        public MultiCalculator(IAsyncPointsIterator iterator, int threadsCount)
        {
            this.iterator = iterator;
            this.threadsCount = threadsCount;
            this.points = new LinkedList<Point>();
        }

        public virtual async Task<Circle> CalculateCircleAsync()
        {
            var firstPoints = (await iterator.GetManyAsync(2)).ToArray();
            var nextPointTask = iterator.GetNextAsync();

            var circle = CreateCircle.FromTwoPoints(firstPoints[0], firstPoints[1]);
            points.AddLast(firstPoints[0]);
            points.AddLast(firstPoints[1]);

            var count = iterator.PointsCount;
            var nextPoint = await nextPointTask;

            while (nextPoint != null)
            {
                RaisePointProcessed(this, new OnPointDrawEventArgs(nextPoint));
                nextPointTask = iterator.GetNextAsync();

                if (!circle.ContainsPoint(nextPoint))
                {
                    // Update the circle to contain the new point as well:
                    circle = FindCircleCombination(nextPoint);
                }

                points.AddFirst(nextPoint);
                nextPoint = await nextPointTask;
            }
            return circle;
        }

        protected Circle FindCircleCombination(Point newPoint)
        {
            var paralelOptions = new ParallelOptions { MaxDegreeOfParallelism = threadsCount };
            Circle minCircle = null;

            var currentThread = 1;


            Parallel.ForEach(points, paralelOptions, (otherPoint, loopstate) =>
            {
                var thread = System.Threading.Thread.CurrentThread.ManagedThreadId;
                var circle = CreateCircle.FromTwoPoints(newPoint, otherPoint);

                if (points.All(circle.ContainsPoint))
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


                Parallel.ForEach(points, paralelOptions, (otherPoint, loopstate) =>
                {
                    var circle = FindCircleThroughTwoPoints(points, newPoint, otherPoint);
                    if (circle < minCircle)
                    {
                        minCircle = circle;
                    }
                });
        
            }

            currentThread++;
            return minCircle;
        }

        private Circle FindCircleThroughTwoPoints(IEnumerable<Point> points, Point point1, Point point2)
        {
            var initial = CreateCircle.FromTwoPoints(point1, point2);

            if (points.All(initial.ContainsPoint))
                return initial;

            Circle left = null;
            Circle right = null;

            foreach (var point3 in points)
            {
                var cross = CrossProduct(point1, point2, point3);
                var c = CreateCircle.TryFromThreePoints(point1, point2, point3);

                if (c == null)
                    continue;
                else if (cross > 0 && (left == null || CrossProduct(point1, point2, c.Center) > CrossProduct(point1, point2, left.Center)))
                    left = c;
                else if (cross < 0 && (right == null || CrossProduct(point1, point2, c.Center) < CrossProduct(point1, point2, right.Center)))
                    right = c;
            }

            return right == null || left != null && left.Radius <= right.Radius ? left : right;
        }


        /// <summary>  Returns twice the signed area of the triangle defined by 3 points </summary>
        double CrossProduct(Point point1, Point point2, Point point3)
        {
            return (point2.X - point1.X) * (point3.Y - point1.Y) - (point2.Y - point1.Y) * (point3.X - point1.X);
        }
    }
}
