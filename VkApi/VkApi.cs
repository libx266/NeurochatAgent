using ApiInterface;
using Data;
using VkNet.Model;

namespace ApiImplements
{
    public class VkApi : IDialogApi, IDisposable
    {
        private readonly VkNet.VkApi _vk;
        public VkApi(string token) : base(token)
        {
            _vk = new VkNet.VkApi();
            _vk.Authorize(new ApiAuthParams { AccessToken = token });
        }

        public override async Task SendMessage(long userId, string message) =>
            await _vk.Messages.SendAsync(new MessagesSendParams { UserId = userId, Message = message, RandomId = Random.Shared.NextInt64() });

        public void Dispose() => _vk.Dispose();

        public override async Task<IEnumerable<MessageDto>> GetMessagesFromUser(long userId, long offset = 0, int count = 200) =>
            (await _vk.Messages.GetHistoryAsync(new MessagesGetHistoryParams { UserId = userId, Offset = offset, Count = count })).Messages.Select(m =>
                new MessageDto(m.ConversationMessageId, m.FromId, m.ChatId, m.Text, m.Date));

        public override async Task<ChatDto> GetChat(long chatId)
        {
            var chat = await _vk.Messages.GetChatAsync(chatId);
            return new ChatDto(chatId, chat.Title);
        }

        public override async Task<UserDto> GetUser(long userId)
        {
            var user = (await _vk.Users.GetAsync(new[] { userId })).FirstOrDefault();
            return new UserDto(userId, user.FirstName, user.LastName);
        }
    }
}