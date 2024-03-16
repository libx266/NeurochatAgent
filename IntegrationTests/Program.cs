using ApiImplements;
using Bot;
int test = 1;

var input = (string promt) =>
{
    Console.Write($"{promt}:  ");
    return Console.ReadLine();
};

if (test == 0)
{
    string token = input("VK Token");
    long id = long.Parse(input("Receipt ID"));

    using var vk = new VkApi(token);
    using var neuro = new TextGenerationWebApi(1, 1, 1);

    await vk.SendMessage(id, await neuro.GenerateBase("Я пришел чтобы купить кролика, что", byte.MaxValue, "\n") ?? "generation error");

}

if (test == 1)
{
    var config = await File.ReadAllLinesAsync(@"C:\Rozpodrawa\neuroagent.txt");
    var bot = new VkBot(config[0], 553916613);
    await Task.WhenAll(bot.PoolUserPrivateMessages(196407990), bot.PoolConference(986, 1), bot.PoolConference(989, 1));
}

