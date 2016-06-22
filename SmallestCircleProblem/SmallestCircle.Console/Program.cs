using SmallestCircle.Calculation;
using SmallestCircle.Data.Input;
using SmallestCircle.Data.Input.File;
using SmallestCircle.Data.Input.Randomized;
using System;

namespace SmallestCircle.ConsoleMode
{
    class Program
    {
        static IAsyncPointsIterator pointsGenerator;

        static DemoCalculator threadCal;

        static void Main(string[] args)
        {
            var arguments = StartArguments.ParseArgs(args);           

            if (!String.IsNullOrEmpty(arguments.PointsFile))
            {
                pointsGenerator = new FilePointsInterator(arguments.PointsFile);
            }
            else
            {
                pointsGenerator = new RandomThreadedPointsGenerator(arguments.PointsCount, 0, 10240);
            }

            threadCal = new DemoCalculator(pointsGenerator, arguments.ThreadCount, arguments.QuietMode);

            threadCal.OnThreadStarted += OnThreadStart;
            threadCal.OnThreadStopped += OnThreadStop;

            threadCal.CalculateCircleAsync().Wait();
            Console.ReadKey();
        }


        protected static void OnThreadStart(object sender, OnThreadStartEventArgs e)
        {
            Console.WriteLine("Thread {0} started", e.ThreadNumber);
        }

        protected static void OnThreadStop(object sender, OnThreadStopEventArgs e)
        {
            Console.WriteLine("Thread {0} stopped", e.ThreadNumber);
        }
    }
}
