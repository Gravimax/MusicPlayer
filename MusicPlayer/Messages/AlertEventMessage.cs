namespace MusicPlayer
{
    public class AlertMessageMessage
    {
        public AlertMessageMessage(string title, string message)
        {
            this.Title = title;
            this.Message = message;
        }

        public readonly string Title;
        public readonly string Message;
    }
}
