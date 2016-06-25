using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmallestCircle.Data.Input.Randomized
{
    public class RandomThreadedPointsGenerator : IAsyncPointsIterator
    {
        private int max;
        private int min;

        readonly ThreadLocal<Random> random =
                 new ThreadLocal<Random>(() => new Random(GetSeed()));

        public int PointsCount { get; set; }

        public RandomThreadedPointsGenerator(int pointsCount, int min, int max)
        {
            this.min = min;
            this.max = max;
            this.PointsCount = pointsCount;
        }

        public Task<IEnumerable<Point?>> GetAllAsync()
        {
            return GetManyAsync(PointsCount);
        }

        public async Task<Point?> GetNextAsync()
        {
            if (PointsCount <= 0)
                return null;

            PointsCount--;
            return await Task.Run(() => new Point(Rand(), Rand()));
        }

        int Rand()
        {
            return random.Value.Next(min, max);
        }

        static int GetSeed()
        {
            return Environment.TickCount * Thread.CurrentThread.ManagedThreadId;
        }

        public async Task<IEnumerable<Point?>> GetManyAsync(int count)
        {
            var tasks = new List<Task<Point?>>(count);

            for (int i = 0; i < count; i++)
            {
                tasks.Add(GetNextAsync());
            }

            return await Task.WhenAll(tasks);
        }
    }
}
