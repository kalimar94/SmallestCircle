using Microsoft.Win32;
using SmallestCircle.Calculation;
using SmallestCircle.Data;
using SmallestCircle.Data.Input.Predefined;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Point = SmallestCircle.Data.Point;
using Path = System.Windows.Shapes.Path;
using System;

namespace SmallestCircle.Presentation
{
    /// <summary>
    /// Interaction logic for ScrachPad.xaml
    /// </summary>
    public partial class ScratchPad : UserControl
    {
        List<Point> points;
        private Path currentCircle;

        public ScratchPad()
        {
            InitializeComponent();
            points = new List<Point>();
        }

        private void DrawingArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(relativeTo: DrawingArea);
            var point = new Point(position.X, position.Y);
            points.Add(point);
            DrawingArea.DrawPoint(point);


            var iterator = new ListPointsIterator(points);
            var calculator = new LinearCalculator(iterator);

            var circle = calculator.CalculateCircle();
            DrawCircle(circle);
        }

        private void DrawCircle(Circle circle)
        {
            if (currentCircle != null)
            {
                DrawingArea.Children.Remove(currentCircle);
            }

            currentCircle = DrawingArea.DrawCircle(circle);
        }

        private void ClearBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            points.Clear();
            DrawingArea.Children.Clear();
        }

        private void ExportBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{points.Count}");

            foreach (var p in points)
            {
                sb.AppendLine($"{Math.Floor(p.X)} {Math.Floor(p.Y)}");
            }

            var dialog = new SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;
                File.WriteAllText(filePath, sb.ToString());
            }
        }
    }
}
