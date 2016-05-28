using System;

namespace SmallestCircle.Data
{
    public class Point
    {
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double DistanceTo(Point other)
        {
            var dx = this.X - other.X;
            var dy = this.Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}
