using SmallestCircle.Data;

namespace SmallestCircle.Calculation.Geometry
{
    public static class CircleExtensions
    {
        public const double Precision = 0.0001;

        public static bool ContainsPoint(this Circle circle, Point point)
        {
            return circle.Center.DistanceTo(point) - circle.Radius < Precision;
        }   
    }
}
