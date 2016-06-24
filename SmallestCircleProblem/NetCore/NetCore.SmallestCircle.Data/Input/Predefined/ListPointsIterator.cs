using System.Collections.Generic;
using System.Linq;

namespace SmallestCircle.Data.Input.Predefined
{
    /// <summary>
    /// Can read and iterate through collection of points from a list
    /// </summary>
    public class ListPointsIterator : IPointsIterator
    {
        private IList<Point> points;
        int counter;

        public ListPointsIterator(IList<Point> points)
        {
            this.points = points;
            counter = 0;
        }

        public int PointsCount => points.Count;

        public IEnumerable<Point> GetAll()
        {
            return points;
        }

        public IEnumerable<Point> GetMany(int count)
        {
            counter += count;
            return points.Take(count);
        }

        public Point GetNext()
        {
            return  counter < points.Count?  points[counter++] : null;
        }
    }
}
