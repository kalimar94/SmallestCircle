using SmallestCircle.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SmallestCircle.Presentation
{
    public static class DrawingExtensions
    {
        public static Path DrawPoint(this IAddChild element, Data.Point point)
        {
            const int PointSize = 2;

            var ellipseGeometry = new EllipseGeometry
            {
                Center = new System.Windows.Point(point.X, point.Y),
                RadiusX = PointSize,
                RadiusY = PointSize,
            };

            var pathData = new Path
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                StrokeThickness = 1,
                Data = ellipseGeometry
            };

            element.AddChild(pathData);
            return pathData;
        }

        public static Path DrawCircle(this IAddChild element, Circle circle)
        {
            if (circle == null)
                return null;

            var ellipseGeometry = new EllipseGeometry
            {
                Center = new System.Windows.Point(circle.Center.X, circle.Center.Y),
                RadiusX = circle.Radius,
                RadiusY = circle.Radius,
            };

            var pathData = new Path()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Data = ellipseGeometry,
                AllowDrop = true
            };

            element.AddChild(pathData);
            return pathData;
        }
    }
}
