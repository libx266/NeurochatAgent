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
    var tasks = config[1].Split(' ').Select(id => bot.PoolUserPrivateMessages(long.Parse(id))).ToList();
    tasks.AddRange(config[3].Split(' ').Select(id => bot.PoolConference(long.Parse(id))));
    await Task.WhenAll(tasks);
}

