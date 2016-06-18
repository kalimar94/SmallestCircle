namespace SmallestCircle.Calculation
{
    public class OnPointDrawEventArgs
    {
      public Data.Point Point { get; set; } 

        public OnPointDrawEventArgs(Data.Point point)
        {
            this.Point = point;
        } 
    }
}
