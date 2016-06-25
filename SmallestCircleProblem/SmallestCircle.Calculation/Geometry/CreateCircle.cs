using SmallestCircle.Data;

namespace SmallestCircle.Calculation.Geometry
{
    public static class CreateCircle
    {

        /// <summary>
        ///  Creates a circle passing through two points, such that the circle diameter is equal to the distance between the points
        /// </summary>
        public static Circle FromTwoPoints(Point firstPoint, Point secondPoint)
        {
            var centerX = (firstPoint.X + secondPoint.X) / 2d;
            var centerY = (firstPoint.Y + secondPoint.Y) / 2d;

            var center = new Point(centerX, centerY);
            var radius = center.DistanceTo(firstPoint);
            return new Circle(center, radius);
        }


        /// <summary>
        /// Creates a Circumscribed circle through 3 points
        /// </summary>
        public static Circle FromThreePoints(Point p1, Point p2, Point p3)
        {
            double x = (p3.X * p3.X * (p1.Y - p2.Y) + (p1.X * p1.X + (p1.Y - p2.Y) * (p1.Y - p3.Y))
                      * (p2.Y - p3.Y) + p2.X * p2.X * (-p1.Y + p3.Y))
                      / (2 * (p3.X * (p1.Y - p2.Y) + p1.X * (p2.Y - p3.Y) + p2.X * (-p1.Y + p3.Y)));
            double y = (p2.Y + p3.Y) / 2 - (p3.X - p2.X) / (p3.Y - p2.Y) * (x - (p2.X + p3.X) / 2);

            var center = new Point(x, y);
            var radius = center.DistanceTo(p1);

            return new Circle(center, radius);
        }

        /// <summary>
        /// Creates a Circumscribed circle through 3 points, however if it is impossible to create the circle returns null
        /// </summary>
        public static Circle TryFromThreePoints(Point p0, Point p1, Point p2)
        {
            double ax = p0.X, ay = p0.Y;
            double bx = p1.X, by = p1.Y;
            double cx = p2.X, cy = p2.Y;

            var d = (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by)) * 2;
            if (d == 0)
                return null;

            var x = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
            var y = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;
            var point = new Point(x, y);

            return new Circle(point, point.DistanceTo(new Point(ax, ay)));
        }
    }
}
