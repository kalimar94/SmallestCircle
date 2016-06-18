namespace SmallestCircle.Calculation
{
    public class OnThreadStartEventArgs
    {
        public int ThreadNumber { get; set; }

        public OnThreadStartEventArgs(int threadNumber)
        {
            this.ThreadNumber = threadNumber;
        }
    }
}
