using System.Linq;

namespace SmallestCircle.ConsoleMode
{
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

        const string DefaultArgs = "-n 10000 -i test.txt";

        public static StartArguments ParseArgs(string[] args)
        {
            int threadCount = 3;
            int pointsCount = 0;
            string pointsFile = null;
            bool quietMode = false;

            if (args.Length == 0)
                args = DefaultArgs.Split(' ');


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
}
