using SmallestCircle.Data;
using System;

namespace SmallestCircle.Calculation.Geometry
{
    public static class PointExtensions
    {
        public static double DistanceTo(this Point self, Point other)
        {
            var dx = self.X - other.X;
            var dy = self.Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
