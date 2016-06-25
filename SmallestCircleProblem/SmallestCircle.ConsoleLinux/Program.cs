using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmallestCircle.ConsoleLinux
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var arguments = StartArguments.ParseArgs(args);

            //var circle = TestLinearCalculator(arguments.PointsFile);

           var circle = TestMultiCalculator(arguments.PointsFile, 1);

            stopWatch.Stop();


            Console.WriteLine("Circle ready center: {0} r: {1}. \n Time: {2} ms",circle.Center, circle.Radius, stopWatch.ElapsedMilliseconds);
            Console.ReadKey();
        }

        public static Circle TestMultiCalculator(string filePath, int threadCount)
        {
            var pointsGenerator = new FilePointsInterator(filePath);
            var multiCalc = new MultiCalculator(pointsGenerator, threadCount);
            var circle = multiCalc.CalculateCircleAsync().Result;

            return circle;
        }

        public static Circle TestLinearCalculator(string filePath)
        {
            var pointGen = new FilePointsInterator(filePath);
            var linealCalc = new Calculator(pointGen);
            var circle = linealCalc.CalculateCircle();
            return circle;
        }


    }
    public class StartArguments
    {
        public int ThreadCount { get; set; }

        public int PointsCount { get; set; }

        public string PointsFile { get; set; }

        public bool QuietMode { get; set; }


        public StartArguments(int threadsCount = 2, int pointsCount = 50, string pointsFile = null, bool quietMode = false)
        {
            this.PointsCount = pointsCount;
            this.ThreadCount = threadsCount;
            this.QuietMode = QuietMode;
            this.PointsFile = pointsFile;
        }

        public static StartArguments ParseArgs(string[] args)
        {
            int threadCount = 3;
            int pointsCount = 0;
            string pointsFile = null;
            bool quietMode = false;

            if (args.Length == 0)
                args = "-n 10000 -i test.txt".Split(' ');


            int count = args.Count();
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    switch (args[i])
                    {
                        case "-n":
                            pointsCount = int.Parse(args[++i]);
                            break;
                        case "-i":
                            pointsFile = args[++i];
                            break;
                        case "-t":
                            threadCount = int.Parse(args[++i]);
                            break;
                        case "-tasks":
                            threadCount = int.Parse(args[++i]);
                            break;
                        case "-q":
                            quietMode = true;
                            break;
                    }
                }
            }

            return new StartArguments(threadCount, pointsCount, pointsFile, quietMode);
        }
    }

    public class FilePointsInterator : IDisposable
    {
        private string fileName;

        private StreamReader stream;

        public int PointsCount { get; private set; }

        public FilePointsInterator(string fileName)
        {
            this.fileName = fileName;
            this.stream = new StreamReader(fileName);
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
            var lines = (await stream.ReadToEndAsync()).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

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

    public class Point
    {
        public double X { get; private set; }

        public double Y { get; private set; }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})",X,Y);
        }
    }
    public class Circle : IComparable<Circle>
    {
        public Circle(Point center, double radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Point Center { get; private set; }

        public double Radius { get; private set; }

        public int CompareTo(Circle other)
        {
            return this.Radius.CompareTo(other.Radius);
        }

        public static bool operator <(Circle first, Circle second)
        {
            return first.CompareTo(second) == -1;
        }

        public static bool operator >(Circle first, Circle second)
        {
            return first.CompareTo(second) == 1;
        }

        public override string ToString()
        {
            return String.Format("({0} {1}) - R: {2}",Center.X,Center.Y,Radius);
        }

    }

    public static class CircleExtensions
    {
        public const double Precision = 0.0001;

        public static bool ContainsPoint(this Circle circle, Point point)
        {
            return circle.Center.DistanceTo(point) - circle.Radius < Precision;
        }

        /// <summary>  Determines weather a circle contains all points in the list </summary>
        public static bool ContainsAllPoints(this Circle circle, IList<Point> points)
        {
            for (int i = points.Count - 1; i >= 0; i--)
            {
                if (!circle.ContainsPoint(points[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
    public static class CreateCircle
    {

        /// <summary>
        ///  Creates a circle passing through two points, such that the circle diameter is equal to the distance between the points
        /// </summary>
        public static Circle FromTwoPoints(Point firstPoint, Point secondPoint)
        {
            var centerX = (firstPoint.X + secondPoint.X) / 2d;
            var centerY = (firstPoint.Y + secondPoint.Y) / 2d;

            var center = new Point(centerX, centerY);
            var radius = center.DistanceTo(firstPoint);
            return new Circle(center, radius);
        }


        /// <summary>
        /// Creates a Circumscribed circle through 3 points
        /// </summary>
        public static Circle FromThreePoints(Point p1, Point p2, Point p3)
        {
            double x = (p3.X * p3.X * (p1.Y - p2.Y) + (p1.X * p1.X + (p1.Y - p2.Y) * (p1.Y - p3.Y))
                      * (p2.Y - p3.Y) + p2.X * p2.X * (-p1.Y + p3.Y))
                      / (2 * (p3.X * (p1.Y - p2.Y) + p1.X * (p2.Y - p3.Y) + p2.X * (-p1.Y + p3.Y)));
            double y = (p2.Y + p3.Y) / 2 - (p3.X - p2.X) / (p3.Y - p2.Y) * (x - (p2.X + p3.X) / 2);

            var center = new Point(x, y);
            var radius = center.DistanceTo(p1);

            return new Circle(center, radius);
        }

        /// <summary>
        /// Creates a Circumscribed circle through 3 points, however if it is impossible to create the circle returns null
        /// </summary>
        public static Circle TryFromThreePoints(Point p0, Point p1, Point p2)
        {
            double ax = p0.X, ay = p0.Y;
            double bx = p1.X, by = p1.Y;
            double cx = p2.X, cy = p2.Y;

            var d = (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by)) * 2;
            if (d == 0)
                return null;

            var x = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
            var y = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;
            var point = new Point(x, y);

            return new Circle(point, point.DistanceTo(new Point(ax, ay)));
        }
    }
    public static class PointExtensions
    {
        /// <summary>
        /// Calculates Eucliden distance between the given point and another point
        /// </summary>
        public static double DistanceTo(this Point self, Point other)
        {
            var dx = self.X - other.X;
            var dy = self.Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
    public class MultiCalculator
    {
        protected FilePointsInterator iterator;
        protected List<Point> points;
        protected int threadsCount;


        public MultiCalculator(FilePointsInterator iterator, int threadsCount)
        {
            this.iterator = iterator;
            this.threadsCount = threadsCount;
            this.points = new List<Point>(iterator.PointsCount);
        }

        public async Task<Circle> CalculateCircleAsync()
        {
            var firstPoints = (await iterator.GetManyAsync(2)).ToArray();
            var nextPointTask = iterator.GetNextAsync();

            var circle = CreateCircle.FromTwoPoints(firstPoints[0], firstPoints[1]);
            points.AddRange(firstPoints);

            var count = iterator.PointsCount;
            var nextPoint = await nextPointTask;

            while (nextPoint != null)
            {
                nextPointTask = iterator.GetNextAsync();

                if (!circle.ContainsPoint(nextPoint))
                {
                    // Update the circle to contain the new point as well:
                    circle = FindCircleCombination(nextPoint);
                }

                points.Add(nextPoint);
                nextPoint = await nextPointTask;
            }
            return circle;
        }

        private Circle FindCircleCombination(Point newPoint)
        {
            var paralelOptions = new ParallelOptions { MaxDegreeOfParallelism = threadsCount };
            Circle minCircle = null;

            // Try all circles through two points - newPoint and one of the rest
            Parallel.ForEach(points, paralelOptions, (otherPoint, loopstate) =>
            {
                var circle = CreateCircle.FromTwoPoints(newPoint, otherPoint);

                if (circle.ContainsAllPoints(points))
                {
                    if (minCircle == null || circle < minCircle)
                    {
                        minCircle = circle;
                        loopstate.Stop();
                    }
                }
            });


            // If no circles are found yet, try all circles through nextPoint and two other points
            if (minCircle == null)
            {
                Parallel.For(0, points.Count, paralelOptions, (i, loopstate) =>
                {
                    for (int j = i + 1; j < points.Count; j++)
                    {
                        var circle = CreateCircle.FromThreePoints(newPoint, points[i], points[j]);

                        if (circle.ContainsAllPoints(points))
                        {
                            if (minCircle == null || circle < minCircle)
                            {
                                minCircle = circle;
                            }
                        }
                    }
                });
            }

            return minCircle;
        }
    }

    public class Calculator
    {
        private FilePointsInterator iterator;
        private List<Point> points;


        public Calculator(FilePointsInterator iterator)
        {
            this.iterator = iterator;
            this.points = new List<Point>(iterator.PointsCount);
        }

        public Circle CalculateCircle()
        {
            var firstPoints = iterator.GetMany(2).ToArray();
            var circle = CreateCircle.FromTwoPoints(firstPoints[0], firstPoints[1]);

            points.AddRange(firstPoints);
            var nextPoint = iterator.GetNext();

            while (nextPoint != null)
            {
                if (!circle.ContainsPoint(nextPoint))
                {
                    // Update the circle to contain the new point as well:
                    circle = FindCircleCombination(nextPoint, points);
                }

                points.Add(nextPoint);
                nextPoint = iterator.GetNext();
            }
            return circle;
        }

        private Circle FindCircleCombination(Point newPoint, List<Point> existingPoints)
        {
            Circle minCircle = null;

            // Try all circles that are formed as a combination of the new point and one of the existing ones
            foreach (var otherPoint in existingPoints)
            {
                var circle = CreateCircle.FromTwoPoints(newPoint, otherPoint);

                if (existingPoints.All(circle.ContainsPoint))
                {
                    if (minCircle == null || circle < minCircle)
                        return circle;
                }
            }

            // Try all circles that are formed as a combination of the new point and two of the existing ones

            for (int i = 0; i < existingPoints.Count; i++)
            {
                for (int j = i + 1; j < existingPoints.Count; j++)
                {
                    var circle = CreateCircle.FromThreePoints(newPoint, existingPoints[i], existingPoints[j]);

                    if (existingPoints.All(circle.ContainsPoint))
                    {
                        if (minCircle == null || circle < minCircle)
                            minCircle = circle;
                    }
                }
            }

            return minCircle;
        }
    }
}
