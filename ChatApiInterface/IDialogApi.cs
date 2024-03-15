namespace ApiInterface
{
    public abstract class IDialogApi
    {
        protected IDialogApi (string token) { }

        public abstract Task SendMessage(long userId, string message);
    }
}