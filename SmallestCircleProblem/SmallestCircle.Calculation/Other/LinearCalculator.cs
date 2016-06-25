namespace SmallestCircle.Calculation
{
    using Geometry;
    using Data;
    using Data.Input;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    /// <summary>
    /// This calculator uses the famous algorithm of Emo Welzl, claiming to find the smallest enclosing circle in linear time
    /// <para> This algorithm is based on linear programming, at each iteration the solution is built ontop of the previous one </para>
    /// <para> It cannot be used as base for multi-threaded algorithms </para>
    /// </summary>
    public class LinearCalculator
    {
        private IPointsIterator iterator;
        public List<Point> Points { get; private set; }


        public LinearCalculator(IPointsIterator iterator)
        {
            this.iterator = iterator;
            this.Points = new List<Point>(iterator.PointsCount);
        }

        public Circle CalculateCircle()
        {
            if (iterator.PointsCount < 2)
                return null;

            var firstPoints = iterator.GetMany(count: 2).ToArray();
            var circle = CreateCircle.FromTwoPoints(firstPoints[0], firstPoints[1]);

            Points.AddRange(firstPoints);
            var nextPoint = iterator.GetNext();

            while (nextPoint != null)
            {
                if (!circle.ContainsPoint(nextPoint))
                {
                    circle = MakeCircleOnePoint(nextPoint);
                }

                Points.Add(nextPoint);
                nextPoint = iterator.GetNext();
            }

            return circle;
        }


        private Circle MakeCircleOnePoint(Point point)
        {
            var circle = new Circle(point, 0);

            for (int i = 0; i < Points.Count; i++)
            {
                if (!circle.ContainsPoint(Points[i]))
                {
                    if (circle.Radius == 0)
                        circle = CreateCircle.FromTwoPoints(point, Points[i]);
                    else
                        circle = FindCircleThroughTwoPoints(Points.Take(i), Points[i], point);
                }
            }
            return circle;
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
