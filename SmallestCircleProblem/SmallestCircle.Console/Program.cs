using SmallestCircle.Calculation;
using SmallestCircle.Data.Input;
using SmallestCircle.Data.Input.File;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;

namespace SmallestCircle.Console
{
    class Program
    {
        public static IAsyncPointsIterator pointsGenerator { get; set; }
        public static DemoCalculator ThreadCal { get; set; }

        static void Main(string[] args)
        {
            var threadCount = 3;
            int pointsCount = 0;
            var pointsFile = String.Empty;
            var quietMode = false;


            if (args.Length == 0)
            {
                args = ConfigurationManager.AppSettings["Args"].Split(' ');
            }

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
                            pointsFile = args[i++];
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

            if (!String.IsNullOrEmpty(pointsFile))
            {
                pointsGenerator = new FilePointsInterator(pointsFile);
            }
            else
            {
                pointsGenerator = new RandomThreadedPointsGenerator(pointsCount, 0, 10240);
            }

            ThreadCal = new DemoCalculator(pointsGenerator, threadCount, quietMode);

            ThreadCal.OnThreadStarted += OnThreadStart;
            ThreadCal.OnThreadStopped += OnThreadStop;

            MainAsync().Wait();


            System.Console.ReadKey();
        }

        static async Task MainAsync()
        {
            await ThreadCal.CalculateCircleAync();
        }


        protected static void OnThreadStart(object sender, OnThreadStartEventArgs e)
        {
            System.Console.WriteLine("Thread {0} started", e.ThreadNumber);
        }

        protected static void OnThreadStop(object sender, OnThreadStopEventArgs e)
        {
            System.Console.WriteLine("Thread {0} stopped", e.ThreadNumber);
        }
    }
}
