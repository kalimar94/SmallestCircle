using SmallestCircle.Calculation;
using SmallestCircle.Data;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SmallestCircle.Presentation
{
    public partial class MainWindow : Window
    {
        const int Offset = 50;
        private Path currentCircle;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var max = Math.Min(DrawingArea.ActualWidth, DrawingArea.ActualHeight) - Offset;
            var generator = new RandomPointGenerator(int.Parse(pointsCountBox.Text), Offset, (int)max);
            var calculator = new Calculator(generator);

            calculator.OnPointProcessed += OnPointDraw;
            calculator.OnCircleFound += OnCircleDraw;

            calculator.CalculateCircle();
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
        RandomThreadedPointsGenerator pointGenerator;

        private async void buttonAync_Click(object sender, RoutedEventArgs e)
        {
            if (calculator == null)
            {
                var max = Math.Min(DrawingArea.ActualWidth, DrawingArea.ActualHeight) - Offset;
                pointGenerator = new RandomThreadedPointsGenerator(int.Parse(pointsCountBox.Text), Offset, (int)max);
                calculator = new DemoCalculator(pointGenerator, 4,false);

                calculator.OnPointProcessed += OnPointDraw;
                calculator.OnCircleFound += OnCircleDraw;
            }

            if (cancelToken == null)
            {
                cancelToken = new CancellationTokenSource();
                buttonAync.Content = "Stop";
                pointsCountBox.IsEnabled = false;
                await calculator.CalculateCircleAsync(cancelToken.Token);
            }
            else
            {
                cancelToken.Cancel();
                cancelToken = null;
                buttonAync.Content = "Start";
            }
        }
    }
}
