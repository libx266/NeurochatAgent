using ApiImplements;

int test = 0;

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
