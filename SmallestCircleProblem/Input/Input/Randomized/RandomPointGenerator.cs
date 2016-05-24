using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallestCircle.Data.Input.Randomized
{
    public class RandomPointGenerator : IPointsIterator
    {
        protected Random generator;
        
        protected int max;
        protected int min;
        protected int pointsCount;

        public RandomPointGenerator(int pointsCount, int min, int max)
        {
            this.pointsCount = pointsCount;
            this.min = min;
            this.max = max;
            this.generator = new Random();
        }

        public IEnumerable<Point> GetAll()
        {
            for (int i = 0; i < pointsCount; i++)
            {
                yield return GetNext();
            }
        }

        public Point GetNext()
        {
            var x = generator.Next(min, max);
            var y = generator.Next(min, max);
            return new Point(x, y);
        }
    }
}
