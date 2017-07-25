using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SashaGrayBot
{
    class TelegramBot
    {
        private readonly string baseAdress;
        private readonly string port;
        private readonly string botToken;
        private BackgroundWorker bw;

        public TelegramBot()
        {
            baseAdress = BotSettings.Default["Host"].ToString();
            port = BotSettings.Default["Port"].ToString();
            botToken = BotSettings.Default["BotToken"].ToString();  
            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
        }

        public void Start()
        {
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync(botToken);
            }
            
        }               
        private async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as string;          
            var client = new TelegramBotClient(key);
            try
            {
                await client.SetWebhookAsync("");
                client.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
                {
                    var message = ev.CallbackQuery.Message;
                    if (ev.CallbackQuery.Data == "callback1")
                    {
                        await Commands.UpdateDev.StartDev(client, message, baseAdress, port);
                    }
                    else if (ev.CallbackQuery.Data == "callback2")
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Не в этот раз(", replyToMessageId: message.MessageId);
                        await client.AnswerCallbackQueryAsync(ev.CallbackQuery.Id);
                    }
                };

                client.OnUpdate += async (object su, Telegram.Bot.Args.UpdateEventArgs evu) =>
                {
                    if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null) return;
                    var update = evu.Update;
                    var message = update.Message;
                    if (message == null) return;
                    if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                    {
                        if (message.Text.ToLower() == "привет")
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, "Привет");
                        }
                        if (message.Text == "/UpdateDev")
                        {
                            var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(
                                                    new Telegram.Bot.Types.InlineKeyboardButton[][]
                                                    {
                                                            new [] {
                                                                new Telegram.Bot.Types.InlineKeyboardButton("Продолжим","callback1"),
                                                                new Telegram.Bot.Types.InlineKeyboardButton("Отмена","callback2"),
                                                            },
                                                    }
                                                );

                            await client.SendTextMessageAsync(message.Chat.Id, "Будем Обновлять?", false, false, 0, keyboard, Telegram.Bot.Types.Enums.ParseMode.Default);
                        }
                        if (message.Text == "Саша обнови")
                        {
                            await Commands.UpdateDev.StartDev(client, message, baseAdress, port);
                        }
                        if (message.Text == "видос")
                        {
                            await client.SendVideoAsync(message.Chat.Id, @"video.mp4");
                        }
                    }
                };
                client.StartReceiving();
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }       
    }
}
