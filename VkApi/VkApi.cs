using ApiInterface;
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
    }
}