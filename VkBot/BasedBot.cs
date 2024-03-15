using ApiInterface;
using Data;
using Data.Database;
using Data.Database.Entities;
using Data.Database.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model;

namespace Bot
{
    public abstract class BasedBot
    {
        private readonly IDialogApi _dialog;
        private readonly INeuroApi _neuro;
        private readonly ApiService _apiService;
        private readonly Func<PromptMessageDto, string> _propmptBuilder;

        public BasedBot(IDialogApi dialog, INeuroApi neuro, ApiService apiService, Func<PromptMessageDto, string> promptBuilder)
        {
            _dialog = dialog;
            _neuro = neuro;
            _apiService = apiService;
            _propmptBuilder = promptBuilder;
        }

        public async Task PoolUserPrivateMessages(long userId, TimeSpan delay, CancellationToken? cancellation = default)
        {
            while (true)
            {
                try
                {
                    cancellation?.ThrowIfCancellationRequested();


                    using var db = new DataContext();

                    var messages = (await _dialog.GetMessagesFromUser(userId)).OrderBy(m => m.SenderDate).ToList();

                    var users = new Dictionary<long, UserModel>();

                    var last = messages.LastOrDefault();
                    var first = messages.FirstOrDefault();
                    if (last != default)
                    {
                        long lastId = last.Id.Value;
                        long firstId = first.Id.Value;
                        long? chatId = last.ChatId;

                        foreach (var m in messages.DistinctBy(m => m.SenderId))
                        {
                            var user = (await db.Users.FirstOrDefaultAsync(u => u.ExternalId == m.SenderId));
                            if (user is null)
                            {
                                var userInfo = await _dialog.GetUser(m.SenderId.Value);
                                user = new UserModel
                                {
                                    ExternalId = m.SenderId,
                                    FirstName = userInfo.FirstName,
                                    LastName = userInfo.LastName,
                                    UserType = UserType.User
                                };
                                db.Add(user);
                                await db.SaveChangesAsync();
                            }
                            users[m.SenderId.Value] = user;

                        }

                        var chat = (await db.Chats.FirstOrDefaultAsync(c => c.ExternalId == chatId || c.ExternalId == userId));
                        if (chat is null)
                        {
                            var chatInfo = chatId is null ? new ChatDto(userId, $"{users[userId].FirstName} {users[userId].LastName}") : await _dialog.GetChat(chatId.Value);
                            chat = new ChatModel
                            {
                                ApiService = _apiService,
                                ExternalId = chatId ?? userId,
                                Name = chatInfo.Name
                            };
                            db.Add(chat);
                            await db.SaveChangesAsync();
                        }

                        var getMessages = () => db.Messages.Where(m => m.ExternalId >= firstId && m.Chat.ExternalId == chat.ExternalId);

                        var oldMessages = getMessages().Select(m => m.ExternalId).ToHashSet();



                        var insert = (from newMessage in messages
                                      where !oldMessages.Contains(newMessage.Id)
                                      orderby newMessage.SenderDate
                                      select new MessageModel
                                      {
                                          Chat = chat,
                                          Sender = users[newMessage.SenderId.Value],
                                          SenderDate = newMessage.SenderDate ?? DateTime.Now,
                                          ExternalId = newMessage.Id.Value,
                                          Text = newMessage.Text,
                                      }
                                     ).ToList();

                        db.Messages.AddRange(insert);
                        await db.SaveChangesAsync();


                        if (insert.LastOrDefault()?.Sender.ExternalId == userId)
                        {
                            string promt = String.Join("\n", getMessages().OrderBy(m => m.SenderDate).Select(m => _propmptBuilder(new PromptMessageDto(m.Sender.FirstName, m.Sender.LastName, m.Text))));
                            var self = users.FirstOrDefault(kv => kv.Key != userId).Value;
                            promt += "\n" + _propmptBuilder(new PromptMessageDto(self?.FirstName ?? "John", self?.LastName ?? "Doe", ""));
                            var responce = await _neuro.GenerateBase(promt, 512, "\n");
                            if (!string.IsNullOrEmpty(responce))
                            {
                                await _dialog.SendMessage(userId, responce);
                            }
                        }




                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
                }
                await Task.Delay(delay);
            }
        }
    }
}
