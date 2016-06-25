using Cudafy;

namespace SmallestCircle.Data
{
    [Cudafy]
    public struct Point
    {
        public double X { get; private set; }

        public double Y { get; private set; }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
