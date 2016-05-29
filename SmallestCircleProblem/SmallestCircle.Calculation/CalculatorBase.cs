namespace SmallestCircle.Calculation
{
    public abstract class CalculatorBase
    {
        public delegate void PointProssedHandler(object sender, OnPointDrawEventArgs e);
        public event PointProssedHandler OnPointProcessed;

        public delegate void CircleFoundHandler(object sender, OnCircleDrawEventArgs e);
        public event CircleFoundHandler OnCircleFound;

        protected virtual void PointProcessed(object sender, OnPointDrawEventArgs e)
        {
            OnPointProcessed?.Invoke(this, e);
        }

        protected virtual void CircleFound(object sender, OnCircleDrawEventArgs e)
        {
            OnCircleFound?.Invoke(this, e);
        }

    }
}
