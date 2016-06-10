namespace SmallestCircle.Calculation
{
    public abstract class CalculatorBase
    {
        public delegate void PointProssedHandler(object sender, OnPointDrawEventArgs e);
        public event PointProssedHandler OnPointProcessed;

        public delegate void CircleFoundHandler(object sender, OnCircleDrawEventArgs e);
        public event CircleFoundHandler OnCircleFound;

        public delegate void ThreadStartedHandler(object sender, OnThreadStartEventArgs e);
        public event ThreadStartedHandler OnThreadStarted;

        public delegate void ThreadStoppedHandler(object sender, OnThreadStopEventArgs e);
        public event ThreadStoppedHandler OnThreadStopped;

        protected virtual void RaisePointProcessed(object sender, OnPointDrawEventArgs e)
        {
            OnPointProcessed?.Invoke(this, e);
        }

        protected virtual void RaiseCircleFound(object sender, OnCircleDrawEventArgs e)
        {
            OnCircleFound?.Invoke(this, e);
        }

        protected virtual void RaiseThreadStarted(object sender, OnThreadStartEventArgs e)
        {
            OnThreadStarted?.Invoke(this, e);
        }

        protected virtual void RaiseThreadStopped(object sender, OnThreadStopEventArgs e)
        {
            OnThreadStopped?.Invoke(this, e);
        }

    }
}
