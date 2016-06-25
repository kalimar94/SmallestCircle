using SmallestCircle.Calculation;
using SmallestCircle.Data;
using SmallestCircle.Data.Input.File;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Diagnostics;

namespace SmallestCircle.ConsoleMode
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = StartArguments.ParseArgs(args);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var circle = CalculateCircle(arguments);
            stopWatch.Stop();

            Console.WriteLine($"Circle ready center: {circle.Center} r: {circle.Radius}. \n Time: {stopWatch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }

        public static Circle CalculateCircle(StartArguments args)
        {
            if (!string.IsNullOrWhiteSpace(args.PointsFile))
            {
                var generator = new FilePointsInterator(args.PointsFile);
                if (!args.QuietMode)
                {
                    var calculator = new DemoCalculator(generator, args.ThreadCount);
                    calculator.OnNotification += (sender, e) => Console.WriteLine(e.Message);
                    return calculator.CalculateCircleAsync().Result;
                }
                else if (args.ThreadCount < 2)
                {
                    var calculator = new Calculator(generator);
                    return calculator.CalculateCircle();
                }
                else
                {
                    var calculator = new MultiCalculator(generator, args.ThreadCount);
                    return calculator.CalculateCircleAsync().Result;
                }
            }
            else
            {
                if (!args.QuietMode)
                {
                    var generator = new RandomThreadedPointsGenerator(args.PointsCount, 0, 47483647);
                    var calculator = new DemoCalculator(generator, args.ThreadCount);
                    calculator.OnNotification += (sender, e) => Console.WriteLine(e.Message);
                    return calculator.CalculateCircleAsync().Result;
                }
                else if (args.ThreadCount < 2)
                {
                    var generator = new RandomPointGenerator(args.PointsCount, 0, 47483647);
                    var calculator = new LinearCalculator(generator);
                    return calculator.CalculateCircle();
                }
                else
                {
                    var generator = new RandomThreadedPointsGenerator(args.PointsCount, 0, 47483647);
                    var calculator = new MultiCalculator(generator, args.ThreadCount);
                    return calculator.CalculateCircleAsync().Result;
                }   
            }
        }
    }
}
