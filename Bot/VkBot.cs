using ApiImplements;
using ApiInterface;
using Bot;
using Data;

namespace Bot
{
    public class VkBot : BasedBot
    {
        private readonly long _self;
        public VkBot(string token, long selfId) : base(new VkApi(token), new TextGenerationWebApi(0.85f, 1.1f, 0.9f), Data.Database.Entities.Enums.ApiService.VK, m => $"{m.FirstName} {m.LastName}:  {m.Text.Substring(0, m.Text.Length < byte.MaxValue ? m.Text.Length : byte.MaxValue)}") => _self = selfId;

        protected override long Self => _self;
    }
}