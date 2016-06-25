using System;

namespace SmallestCircle.Data
{
    public class Circle : IComparable<Circle>
    {
        public Circle(Point center, double radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Point Center { get; private set; }

        public double Radius { get; private set; }

        public int CompareTo(Circle other)
        {
            return this.Radius.CompareTo(other.Radius);
        }

        public static bool operator < (Circle first, Circle second)
        {
            return first.CompareTo(second) == -1;
        }

        public static bool operator > (Circle first, Circle second)
        {
            return first.CompareTo(second) == 1;
        }

        public override string ToString()
        {
            return $"({Center.X} {Center.Y}) - R: {Radius}";
        }
    }
}
