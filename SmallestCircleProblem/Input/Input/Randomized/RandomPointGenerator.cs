using System;
using System.Collections.Generic;

namespace SmallestCircle.Data.Input.Randomized
{
    public class RandomPointGenerator : IPointsIterator
    {
        private Random generator;

        private int max;
        private int min;

        public RandomPointGenerator(int pointsCount, int min, int max)
        {
            this.PointsCount = pointsCount;
            this.min = min;
            this.max = max;
            this.generator = new Random();
        }

        public int PointsCount { get; private set; }

        public IEnumerable<Point> GetAll()
        {
            for (int i = 0; i < PointsCount; i++)
            {
                yield return GetNext();
            }
        }

        public IEnumerable<Point> GetMany(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return GetNext();
            }
        }

        public Point GetNext()
        {
            if (PointsCount < 1)
                return null;

            PointsCount--;
            var x = generator.Next(min, max);
            var y = generator.Next(min, max);
            return new Point(x, y);
        }
    }
}
