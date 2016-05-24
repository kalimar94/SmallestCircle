using System;

namespace SmallestCircle.Data
{
    public class Circle
    {
        public Circle(Point center, double radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Point Center { get; private set; }

        public double Radius { get; private set; }

        public static Circle FromTwoPoints(Point firstPoint, Point secondPoint)
        {
            var centerX = (firstPoint.X + secondPoint.X) / 2d;
            var centerY = (firstPoint.Y + secondPoint.Y) / 2d;

            var center = new Point(centerX, centerY);
            var radius = center.DistanceTo(firstPoint);
            return new Circle(center, radius);
        }

        public static Circle FromThreePoints(Point first, Point second, Point third)
        {
            var offset = second.X * second.X + second.Y * second.Y;
            var bc = (first.X * first.X + first.Y + first.Y - offset) / 2d;
            var cd = (offset - third.X * third.X - third.Y * third.Y) / 2d;
            var det = (first.X - second.X) * (second.Y - third.Y) - (second.X - third.X) * (first.Y - second.Y);
            var iDet = 1 / det;

            var centerX = (bc * (second.Y - third.Y) - cd * (first.Y - second.Y)) * iDet;
            var centerY = (cd * (first.X - second.X) - bc * (second.X - third.X)) * iDet;
            var radius = Math.Sqrt(Math.Pow(second.X - centerX, 2) + Math.Pow(second.Y - centerY, 2));

            return new Circle(new Point(centerX, centerY), radius);
        }
    }
}
