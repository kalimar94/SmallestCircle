using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmallestCircle.Data.Input.File
{
    public class FilePointsInterator : IPointsIterator, IAsyncPointsIterator, IDisposable
    {
        private string fileName;

        private StreamReader stream;

        public FilePointsInterator(string fileName)
        {
            this.fileName = fileName;
            this.stream = new StreamReader(fileName);
            stream.ReadLine();
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
    }
}
