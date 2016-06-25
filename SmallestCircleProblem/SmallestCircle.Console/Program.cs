using SmallestCircle.Calculation;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using SmallestCircle.Data.Input.File;
using System;
using System.Diagnostics;

namespace SmallestCircle.ConsoleMode
{
    class Program
    {
        static IAsyncPointsIterator pointsGenerator;

        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var arguments = StartArguments.ParseArgs(args);

            //var circle = TestLinearCalculator(arguments.PointsFile);

            //var circle = TestDemoCalculator(arguments);
            var circle = TestMultiCalculator(arguments.PointsFile, arguments.ThreadCount);

            stopWatch.Stop();


            Console.WriteLine($"Circle ready center: {circle.Center} r: {circle.Radius}. \n Time: {stopWatch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }

        public static Circle TestLinearCalculator(string filePath)
        {
            IPointsIterator pointGen = new FilePointsInterator(filePath);
            var linealCalc = new LinearCalculator(pointGen);
            var circle = linealCalc.CalculateCircle();
            return circle;
        }

        public static Circle TestMultiCalculator(string filePath, int threadCount)
        {
            pointsGenerator = new FilePointsInterator(filePath);
            var multiCalc = new MultiCalculator(pointsGenerator, threadCount);
            var circle = multiCalc.CalculateCircleAsync().Result;

            return circle;
        }

        public static Circle TestDemoCalculator(StartArguments args)
        {
            pointsGenerator = new FilePointsInterator(args.PointsFile);
            var calc = new DemoCalculator(pointsGenerator, args.ThreadCount);

            if (!args.QuietMode)
                calc.OnNotification += (sender, e) => Console.WriteLine(e.Message);

            var circle = calc.CalculateCircleAsync().Result;
            return circle;
        }
    }
}
