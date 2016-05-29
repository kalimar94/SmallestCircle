using SmallestCircle.Calculation;
using SmallestCircle.Data;
using SmallestCircle.Data.Input.Randomized;
using System;
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
        private Calculator calculator;
        private RandomPointGenerator generator;

        public MainWindow()
        {
            InitializeComponent();
            generator = new RandomPointGenerator(10, 10, 450);
            calculator = new Calculator(generator);

            calculator.OnPointProcessed += OnPointDraw;
            calculator.OnCircleFound += OnCircleDraw;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
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

            DrawingArea.Children.Add(myPath);
        }
    }
}
