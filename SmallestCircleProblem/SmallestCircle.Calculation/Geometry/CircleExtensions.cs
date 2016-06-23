using SmallestCircle.Data;
using System.Collections.Generic;

namespace SmallestCircle.Calculation.Geometry
{
    public static class CircleExtensions
    {
        public const double Precision = 0.0001;

        public static bool ContainsPoint(this Circle circle, Point point)
        {
            return circle.Center.DistanceTo(point) - circle.Radius < Precision;
        }

        /// <summary>  Determines weather a circle contains all points in the list </summary>
        public static bool ContainsAllPoints(this Circle circle, IList<Point> points)
        {
            for (int i = points.Count - 1; i >= 0; i--)
            {
                if (!circle.ContainsPoint(points[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
