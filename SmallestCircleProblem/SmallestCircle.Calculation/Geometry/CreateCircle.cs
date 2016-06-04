using SmallestCircle.Data;
using System.Collections.Generic;

namespace SmallestCircle.Calculation.Geometry
{
    public static class CreateCircle
    {
        public static ICollection<Point> LastFormingPoints { get; set; }


        public static Circle FromTwoPoints(Point firstPoint, Point secondPoint)
        {
            LastFormingPoints = new[] { firstPoint, secondPoint };

            var centerX = (firstPoint.X + secondPoint.X) / 2d;
            var centerY = (firstPoint.Y + secondPoint.Y) / 2d;

            var center = new Point(centerX, centerY);
            var radius = center.DistanceTo(firstPoint);
            return new Circle(center, radius);
        }


        public static Circle FromThreePoints(Point p1, Point p2, Point p3)
        {
            LastFormingPoints = new[] { p1, p2, p3 };

            double x = (p3.X * p3.X * (p1.Y - p2.Y) + (p1.X * p1.X + (p1.Y - p2.Y) * (p1.Y - p3.Y))
                      * (p2.Y - p3.Y) + p2.X * p2.X * (-p1.Y + p3.Y))
                      / (2 * (p3.X * (p1.Y - p2.Y) + p1.X * (p2.Y - p3.Y) + p2.X * (-p1.Y + p3.Y)));
            double y = (p2.Y + p3.Y) / 2 - (p3.X - p2.X) / (p3.Y - p2.Y) * (x - (p2.X + p3.X) / 2);

            var center = new Point(x, y);
            var radius = center.DistanceTo(p1);

            return new Circle(center, radius);
        }
    }
}
