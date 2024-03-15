using ApiImplements;
using ApiInterface;
using Bot;
using Data;

namespace Bot
{
    public class VkBot : BasedBot
    {
        public VkBot(string token) : base(new VkApi(token), new TextGenerationWebApi(0.85f, 1.1f, 0.9f), Data.Database.Entities.Enums.ApiService.VK, m => $"{m.FirstName} {m.LastName}:  {m.Text}") { }

        
    }
}