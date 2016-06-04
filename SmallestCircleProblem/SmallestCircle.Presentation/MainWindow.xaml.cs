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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                var max = Math.Min(DrawingArea.ActualWidth, DrawingArea.ActualHeight) - 50;
                var generator = new RandomPointGenerator(1000, 50, (int)max);
                var calculator = new Calculator(generator);

                calculator.OnPointProcessed += OnPointDraw;
                calculator.OnCircleFound += OnCircleDraw;

                generator.PointsCount = 13;
                calculator.CalculateCircle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
       
        protected void OnPointDraw(object sender, OnPointDrawEventArgs e)
        {
            DrawPoint(e.Point);
        }

        protected void OnCircleDraw(object sender, OnCircleDrawEventArgs e)
        {
            if (CurrentCircle != null)
            {
                DrawingArea.Children.Remove(CurrentCircle);
            }

            DrawCircle(e.Circle);
        }

        public void DrawPoint(Data.Point point)
        {
            var circle = new Circle(point, 2);

            var myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = new System.Windows.Point(circle.Center.X, circle.Center.Y);
            myEllipseGeometry.RadiusX = circle.Radius;
            myEllipseGeometry.RadiusY = circle.Radius;

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.Fill = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myEllipseGeometry;

            DrawingArea.Children.Add(myPath);
        }

        private static Path CurrentCircle;


        public void DrawCircle(Data.Circle circle)
        {
            var myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = new System.Windows.Point(circle.Center.X, circle.Center.Y);
            myEllipseGeometry.RadiusX = circle.Radius;
            myEllipseGeometry.RadiusY = circle.Radius;

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myEllipseGeometry;

            CurrentCircle = myPath;
            DrawingArea.Children.Add(CurrentCircle);
        }


        CancellationTokenSource cancelToken;
        DemoCalculator calculator;


        private async void buttonAync_Click(object sender, RoutedEventArgs e)
        {
            if (calculator == null)
            {
                var max = Math.Min(DrawingArea.ActualWidth, DrawingArea.ActualHeight) - 50;
                var threadGenerator = new RandomThreadedPointsGenerator(1000, 50, (int)max);
                calculator = new DemoCalculator(threadGenerator, 4);

                calculator.OnPointProcessed += OnPointDraw;
                calculator.OnCircleFound += OnCircleDraw;
            }

            if (cancelToken == null)
            {
                cancelToken = new CancellationTokenSource();
                await calculator.CalculateCircleAync(cancelToken.Token);
            }
            else
            {
                cancelToken.Cancel();
                cancelToken = null;
            }        
        }
    }
}
