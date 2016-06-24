namespace SmallestCircle.Calculation
{
    public class NotificationEventArgs
    {
        public string Message { get; private set; }

        public NotificationEventArgs(string message)
        {
            this.Message = message;
        }

        public static implicit operator NotificationEventArgs(string message)
        {
            return new NotificationEventArgs(message);
        }
    }
}
