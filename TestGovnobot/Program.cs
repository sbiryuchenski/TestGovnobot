using Microsoft.VisualBasic;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace TestGovnobot
{
    class Program
    {
        static private string token { get; set; } = "6369101562:AAEEbhMCYupTDlkQzwNckMYtEL_lBWPXSSQ";
        static private List<SendMessage> sendMessages = new List<SendMessage>();
        static void Main(string[] args)
        {
            TelegramBotClient client = new TelegramBotClient(token);

            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        private async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;

            if (message != null)
            {
                await Console.Out.WriteLineAsync($"{message.Chat.Username} | {message.Chat.Id} | {message.Text}");

                bool isSendingMessage = false;
                bool isIdrecieved = false;

                foreach (var sndmsg in sendMessages)
                {
                    if (sndmsg.SenderId == message.Chat.Id)
                    {
                        isSendingMessage = true;
                        if (sndmsg.ChatId != 0)
                            isIdrecieved = true;
                    }
                    break;
                }

                if (isSendingMessage == true && isIdrecieved == false)
                {
                    string recieverId = message.Text;
                    if (message.Text.ToLower() == "назад")
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Ёбаный врот этого казино блять", replyMarkup: GetMainButtons());
                        if (sendMessages.Count != 0) sendMessages.Remove(sendMessages.Single(_ => _.SenderId == message.Chat.Id));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(recieverId))
                        {
                            try
                            {
                                Int64.TryParse(recieverId, out var reciever);
                                foreach (var sndmsg in sendMessages)
                                {
                                    if (sndmsg.SenderId == message.Chat.Id)
                                    {
                                        sndmsg.ChatId = Convert.ToInt64(recieverId);
                                        await botClient.SendTextMessageAsync(message.Chat.Id, "Введи сообщение, которе ты хочешь ему отправить блять", replyMarkup: GetBackButton());
                                    }
                                }
                            }
                            catch
                            {
                                await botClient.SendTextMessageAsync(message.Chat.Id, "Блять сказано же, отправь ID. Если не знаешь id, то иди нахуй вообще", replyMarkup: GetBackButton());
                            }
                        }
                    }
                }



                if (isIdrecieved == true)
                {
                    if (!string.IsNullOrEmpty(message.Text))
                    {
                        if (message.Text.ToLower() == "назад")
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Ёбаный врот этого казино блять", replyMarkup: GetMainButtons());
                            if (sendMessages.Count != 0) sendMessages.Remove(sendMessages.Single(_ => _.SenderId == message.Chat.Id));
                        }
                        else
                        {
                            foreach (var sndmsg in sendMessages)
                            {
                                if (sndmsg.SenderId == message.Chat.Id)
                                {
                                    sndmsg.Message = message.Text;
                                    await botClient.SendTextMessageAsync(sndmsg.ChatId, $"Сообщение от {sndmsg.SenderName}: {sndmsg.Message}", replyMarkup: GetMainButtons());
                                    await Console.Out.WriteLineAsync($"{sndmsg.SenderName} | {sndmsg.SenderId} >> {sndmsg.ChatId} | {sndmsg.Message}");
                                }
                            }
                            if (sendMessages.Count != 0) sendMessages.Remove(sendMessages.Single(_ => _.SenderId == message.Chat.Id));
                        }
                    }
                }

                //if (message.Text.Contains("♂"))
                //{
                //    string msg = message.Text;
                //    var arr = msg.Split("♂");
                //    string id = arr[0];
                //    string txt = arr[1];
                //    await botClient.SendTextMessageAsync(Convert.ToInt64(id), txt, replyMarkup: GetMainButtons());
                //    return;
                //}

                if (isSendingMessage == false && isIdrecieved == false)
                {
                    switch (message.Text.ToLower())
                    {
                        case "отправить сообщение":
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Введи id челика которому хочешь отправить сообщение");
                            sendMessages.Add(new SendMessage());
                            sendMessages[sendMessages.Count - 1].SenderId = message.Chat.Id;
                            sendMessages[sendMessages.Count - 1].SenderName = message.Chat.Username;
                            break;
                        case "пойти нахуй":
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Ну так иди нахуй, хули ты сидишь");
                            break;
                        default:
                            await botClient.SendTextMessageAsync(message.Chat.Id, "На кнопочки блять тыкай, что ты мне хуйню пишешь", replyMarkup: GetMainButtons());
                            break;
                    }
                }
            }
        }

        private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private static IReplyMarkup GetMainButtons()
        {
            return new ReplyKeyboardMarkup(new List<List<KeyboardButton>> { new List<KeyboardButton> { new KeyboardButton("Отправить сообщение"), new KeyboardButton("Пойти нахуй") } })
            {
            };
        }
        private static IReplyMarkup GetBackButton()
        {
            return new ReplyKeyboardMarkup(new List<List<KeyboardButton>> { new List<KeyboardButton> { new KeyboardButton("Назад") } })
            {
            };
        }
    }
}
