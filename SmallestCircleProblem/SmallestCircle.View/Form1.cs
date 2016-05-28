using SmallestCircle.Data;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmallestCircle.View
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var generator = new RandomPointGenerator(5, 10, 12);
            var nextPoint = generator.GetNext();

            var calculator = new Calculation.Calculator(generator);

            var circle = calculator.CalculateCircle();
            var graphic = this.panel1.CreateGraphics(); ;
            try
            {
                var center = circle.Center;
                double radius = (int)circle.Radius;
                double x = center.X - radius;
                double y = center.Y - radius;
                double width = 2 * radius;
                int height = (int)(2 * radius);
                graphic.DrawArc(new Pen(Color.Blue), (float)x, (float)y, (float)width, (float)height, 0, 360);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


      
    }
}
