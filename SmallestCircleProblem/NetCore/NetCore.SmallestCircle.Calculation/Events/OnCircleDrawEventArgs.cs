namespace SmallestCircle.Calculation
{
    public class OnCircleDrawEventArgs
    {
        public Data.Circle Circle { get; set; }

        public OnCircleDrawEventArgs(Data.Circle circle)
        {
            this.Circle = circle;
        } 
    }
}
