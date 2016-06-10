namespace SmallestCircle.Calculation
{
    public class OnThreadStopEventArgs
    {
        public int ThreadNumber { get; set; }

        public OnThreadStopEventArgs(int threadNumber)
        {
            this.ThreadNumber = threadNumber;
        }
    }
}
