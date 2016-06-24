using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmallestCircle.Data.Input.File
{
    /// <summary>
    /// Can read and iterate through collection of points from a file
    /// </summary>
    public class FilePointsInterator : IPointsIterator, IAsyncPointsIterator, IDisposable
    {
        private string fileName;

        private StreamReader stream;

        public int PointsCount { get; private set; }

        public FilePointsInterator(string fileName)
        {
            this.fileName = fileName;
            this.stream = new StreamReader(new FileStream(fileName, FileMode.Open));
            PointsCount = int.Parse(stream.ReadLine());
        }

        public IEnumerable<Point> GetAll()
        {
            var lines = stream.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return lines.Select(ConvertToPoint);
        }

        public Point GetNext()
        {
            return ConvertToPoint(stream.ReadLine());
        }

        static Point ConvertToPoint(string line)
        {
            if (line == null) return null;

            var segments = line.Split(' ').Select(int.Parse);
            var x = segments.First();
            var y = segments.Last();

            return new Point(x, y);
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        public async Task<Point> GetNextAsync()
        {
            return ConvertToPoint(await stream.ReadLineAsync());
        }

        public async Task<IEnumerable<Point>> GetAllAsync()
        {
            var lines = (await stream.ReadToEndAsync())
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            return lines.Select(ConvertToPoint);
        }

        public IEnumerable<Point> GetMany(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return GetNext();
            }
        }

        public async Task<IEnumerable<Point>> GetManyAsync(int count)
        {
            var tasks = new List<Task<Point>>(count);

            for (int i = 0; i < count; i++)
            {
                tasks.Add(GetNextAsync());
            }

            return await Task.WhenAll(tasks);
        }
    }
}
