namespace Web
{
    public class WebRequestToken
    {
        public bool IsCancelled { get; private set; }
        public bool IsCompleted { get; private set; }
        public string ResponseText { get; private set; }

        public void Cancel() => IsCancelled = true;

        public void Complete(string responseText)
        {
            ResponseText = responseText;
            IsCompleted = true;
        }
    }
}
