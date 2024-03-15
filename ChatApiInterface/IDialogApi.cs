using Data;

namespace ApiInterface
{
    public abstract class IDialogApi
    {
        protected IDialogApi (string token) { }

        public abstract Task SendMessage(long userId, string message);

        public abstract Task<IEnumerable<MessageDto>> GetMessagesFromUser(long userId, long offset = 0, int count = 200);

        public abstract Task<ChatDto> GetChat(long chatId);
        public abstract Task<UserDto> GetUser(long userId);
    }
}