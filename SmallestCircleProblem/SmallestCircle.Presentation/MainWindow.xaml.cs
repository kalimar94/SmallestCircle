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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var generator = new RandomPointGenerator(3, 50, 450);
            var allPoints = generator.GetAll();

            var calculator = new Calculation.Calculator(generator);  
                     
          
            try
            {

                foreach (var point in allPoints)
                {
                    var circle = new Circle(point, 2);                    

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
            myPath.StrokeThickness = 1;
            myPath.Data = myEllipseGeometry;

            DrawingArea.Children.Add(myPath);
        }

        public void DrawCircle(Data.Point point, double radius)
        {
            var circle = new Circle(point, radius);

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
