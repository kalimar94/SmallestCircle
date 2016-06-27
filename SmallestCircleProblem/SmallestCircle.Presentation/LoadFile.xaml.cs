using Microsoft.Win32;
using SmallestCircle.Calculation;
using SmallestCircle.Data.Input.File;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SmallestCircle.Presentation
{
    /// <summary>
    /// Interaction logic for LoadFile.xaml
    /// </summary>
    public partial class LoadFile : UserControl
    {
        public LoadFile()
        {
            InitializeComponent();
        }

        const int Offset = 50;
        private Path currentCircle;

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Multiselect = false;

            openFileDialog.Filter = "Text documents (.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
                UploadText.Text = openFileDialog.FileName;

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        DemoCalculator calculator;

        FilePointsInterator pointsIterator;

        private async void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(UploadText.Text))
            {
                pointsIterator = new FilePointsInterator(UploadText.Text);
            }
            else
            {
                MessageBox.Show("There is no file provided");
                return;
            }

            if (calculator == null)
            {
                calculator = new DemoCalculator(pointsIterator, 4);
            }
            calculator.OnPointProcessed += OnPointDraw;
            calculator.OnCircleFound += OnCircleDraw;
            await calculator.CalculateCircleAsync();          

        }


        protected void OnPointDraw(object sender, OnPointDrawEventArgs e)
        {
            DrawingArea.DrawPoint(e.Point);
        }

        protected void OnCircleDraw(object sender, OnCircleDrawEventArgs e)
        {
            if (currentCircle != null)
            {
                DrawingArea.Children.Remove(currentCircle);
            }

            currentCircle = DrawingArea.DrawCircle(e.Circle);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DrawingArea.Children.Clear();
            currentCircle = null;
            calculator = null;
        }
    }
}
