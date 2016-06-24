using SmallestCircle.Calculation;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using SmallestCircle.Data.Input.File;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SmallestCircle.ConsoleMode
{
    class Program
    {
        static IAsyncPointsIterator pointsGenerator;

        static DemoCalculator threadCal;

        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var arguments = StartArguments.ParseArgs(args);

            var circle = TestLinearCalculator(arguments.PointsFile);

            //var circle = TestMultiCalculator(arguments.PointsFile, 3);

            stopWatch.Stop();


            Console.WriteLine($"Circle ready center: {circle.Center} r: {circle.Radius}. \n Time: {stopWatch.ElapsedMilliseconds} ms");

            Console.ReadKey();
        }


        public static Circle TestLinearCalculator(string filePath)
        {
            IPointsIterator pointGen = new FilePointsInterator(filePath);

            var linealCalc = new Calculator(pointGen);

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
