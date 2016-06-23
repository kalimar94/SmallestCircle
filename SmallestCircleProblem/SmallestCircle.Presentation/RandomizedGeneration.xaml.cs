using Microsoft.Win32;
using SmallestCircle.Calculation;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Path = System.Windows.Shapes.Path;

namespace SmallestCircle.Presentation
{
    /// <summary>
    /// Interaction logic for RandomizedGeneration.xaml
    /// </summary>
    public partial class RandomizedGeneration : UserControl
    {
        public RandomizedGeneration()
        {
            InitializeComponent();
        }

        const int Offset = 50;
        private Path currentCircle;


        private void button_Click(object sender, RoutedEventArgs e)
        {
            var max = Math.Min(DrawingArea.ActualWidth, DrawingArea.ActualHeight) - Offset;
            var generator = new RandomPointGenerator(int.Parse(pointsCountBox.Text), Offset, (int)max);
            linearCalculator = new LinearCalculator(generator);

            linearCalculator.OnPointProcessed += OnPointDraw;
            linearCalculator.OnCircleFound += OnCircleDraw;
            
            linearCalculator.CalculateCircle();
        }

        protected void OnPointDraw(object sender, OnPointDrawEventArgs e)
        {
            DrawingArea.DrawPoint(e.Point);
            this.pointsCountBox.Text = pointGenerator?.PointsCount.ToString();
        }

        protected void OnCircleDraw(object sender, OnCircleDrawEventArgs e)
        {
            if (currentCircle != null)
            {
                DrawingArea.Children.Remove(currentCircle);
            }

            currentCircle = DrawingArea.DrawCircle(e.Circle);
        }

        CancellationTokenSource cancelToken;
        DemoCalculator calculator;
        LinearCalculator linearCalculator;

        RandomThreadedPointsGenerator pointGenerator;

        private async void buttonAync_Click(object sender, RoutedEventArgs e)
        {
            if (calculator == null)
            {
                //var max = Math.Min(DrawingArea.ActualWidth, DrawingArea.ActualHeight) - Offset;
                var max = int.MaxValue;
                pointGenerator = new RandomThreadedPointsGenerator(int.Parse(pointsCountBox.Text), Offset, (int)max);
                calculator = new DemoCalculator(pointGenerator, 4, false);

                calculator.OnPointProcessed += OnPointDraw;
                calculator.OnCircleFound += OnCircleDraw;
            }

            if (cancelToken == null)
            {
                pointGenerator.PointsCount = int.Parse(pointsCountBox.Text);
                cancelToken = new CancellationTokenSource();
                buttonAync.Content = "Stop";
                pointsCountBox.IsEnabled = false;
                await calculator.CalculateCircleAsync(cancelToken.Token);
                pointsCountBox.IsEnabled = true;
            }
            else
            {
                cancelToken.Cancel();
                cancelToken = null;
                buttonAync.Content = "Start"; ;
            }
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            var points = calculator?.Points as ICollection<Data.Point> ?? linearCalculator?.Points ;

            if (points == null)
            {
                MessageBox.Show("No points to export");
                return;
            }

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


        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            DrawingArea.Children.Clear();
            currentCircle = null;
            pointGenerator = null;
            calculator = null;
            linearCalculator = null;
        }
    }
}
