using SmallestCircle.Data;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmallestCircle.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var generator = new RandomPointGenerator(2, 10, 12);
            var nextPoint = generator.GetNext();
            var circle = new Circle(nextPoint, 2.4);


        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InputX_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
