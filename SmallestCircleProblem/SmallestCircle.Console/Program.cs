using SmallestCircle.Calculation;
using SmallestCircle.Data.Input;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallestCircle.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var threadCount = 1;
            int pointsCount = 0;
            var pointsFile = String.Empty;
            var quietMode = false;
           
            int count = args.Count();
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    switch (args[i])
                    {
                        case "-n":
                            pointsCount = int.Parse(args[i++]);
                            break;
                        case "-i":
                            pointsFile = args[i++];
                            break;
                        case "-t":
                            threadCount = int.Parse(args[i++]);
                            break;
                        case "-tasks":
                            threadCount = int.Parse(args[i++]);
                            break;
                        case "-q":
                            quietMode = true;
                            break;
                    }
                }
            }

            var pointGenerator = new RandomThreadedPointsGenerator(pointsCount, 0, 10240);
            var threadCal = new DemoCalculator(pointGenerator, threadCount);
        }
    }
}
