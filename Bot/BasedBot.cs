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

        protected abstract long Self { get; }

        private async Task ProcessingMessages(List<MessageDto> messages, ChatModel chat, Func<string, Task> answer, DataContext db, int period = 1)
        {
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

                var users2 = getMessages().Where(m => m.Sender.FirstName != "Bot" && !string.IsNullOrEmpty(m.Text)).OrderByDescending(m => m.SenderDate).Select(m => m.Sender.ExternalId).Take(period).ToHashSet();

                if (!users2.Contains(Self))
                {
                    var names = new HashSet<string>();

                    string promt = String.Join("\n", getMessages().Where(m => m.Sender.FirstName != "Bot" && !string.IsNullOrEmpty(m.Text)).OrderBy(m => m.SenderDate).ToList().Select(m => 
                    {
                        names.Add(m.Sender.FirstName + ' ' + m.Sender.LastName);
                        return _propmptBuilder(new PromptMessageDto(m.Sender.FirstName, m.Sender.LastName, m.Text));
                    }));
                    
                    var self = await  _dialog.GetUser(Self);
                    string selfFirstName = self?.FirstName ?? "John";
                    string selfLastName = self?.LastName ?? "Doe";
                    names.Remove(selfFirstName + ' ' + selfLastName);

                    promt += "\n" + _propmptBuilder(new PromptMessageDto(selfFirstName, selfLastName, ""));
                    
                    var responce = await _neuro.GenerateBase(promt, 40, "\n");
                    
                    if (!(string.IsNullOrEmpty(responce) || responce.Contains("http://") || responce.Contains("https://") || names.Any(n => responce.Contains(n))))
                    {
                        await answer(responce);
                    }
                }

            }
        }

        public async Task PoolUserPrivateMessages(long userId, TimeSpan? delay = default, CancellationToken? cancellation = default)
        {
            await Task.Delay(Random.Shared.Next(500, 2000));
            delay ??= TimeSpan.FromSeconds(2);

            while (true)
            {
                cancellation?.ThrowIfCancellationRequested();
                try
                {
                    using var db = new DataContext();

                    var messages = (await _dialog.GetMessagesFromUser(userId)).OrderBy(m => m.SenderDate).ToList();

                    var chat = (await db.Chats.FirstOrDefaultAsync(c => c.ExternalId == userId));

                    if (chat is null)
                    {
                        var chatInfo = await _dialog.GetUser(userId);

                        chat = new ChatModel
                        {
                            ApiService = _apiService,
                            ExternalId = userId,
                            Name = chatInfo.FirstName + ' ' + chatInfo.LastName,
                        };

                        db.Add(chat);
                        await db.SaveChangesAsync();
                    }

                    await ProcessingMessages(messages, chat, responce => _dialog.SendMessage(userId, responce), db);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
                }

                await Task.Delay(delay.Value);
            }
        }

        public async Task PoolConference(long id, int messagesStep = 3, TimeSpan? delay = default, CancellationToken? cancellation = default)
        {
            await Task.Delay(Random.Shared.Next(500, 2000));
            delay ??= TimeSpan.FromSeconds(2);

            while (true)
            {
                cancellation?.ThrowIfCancellationRequested();
                try
                {
                    using var db = new DataContext();

                    var chat = (await db.Chats.FirstOrDefaultAsync(c => c.ExternalId == id));

                    if (chat is null)
                    {
                        var chatInfo = await _dialog.GetChat(id);

                        chat = new ChatModel
                        {
                            ApiService = _apiService,
                            ExternalId = chatInfo.Id,
                            Name = chatInfo.Name,
                        };

                        db.Add(chat);
                        await db.SaveChangesAsync();
                    }

                    var messages = (await _dialog.GetMessagesFromChat(chat.ExternalId.Value)).OrderBy(m => m.SenderDate).ToList();

                    await ProcessingMessages(messages, chat, responce => _dialog.SendMessageChat(chat.ExternalId.Value, responce), db, messagesStep);


                }
                catch (Exception ex)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
                }

                await Task.Delay(delay.Value);
            }
        }
    }
}
