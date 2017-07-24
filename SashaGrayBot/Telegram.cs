﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Telegram.Bot;

namespace SashaGrayBot
{
    class TelegramBot
    {
        private readonly string botToken;
        private readonly string jenkinsUserName;
        private readonly string jenkinsUserToken;
        private BackgroundWorker bw;

        public TelegramBot()
        {
            botToken = BotSettings.Default["BotToken"].ToString();
            jenkinsUserName = BotSettings.Default["UserName"].ToString();
            jenkinsUserToken = BotSettings.Default["PasswordToken"].ToString();    
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
                            using (var host = new HttpClient())
                            {
                                var credentials = Encoding.ASCII.GetBytes("Sasha:91fd1607341d5a5b723519b218023ad3");
                                host.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
                                host.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");                          
                                var result = await host.GetAsync(@"http://192.168.104.144:8080/job/first/build?token=someAuthorizationFuckingTocketThatICantFindWhereToGenerate");
                                await client.AnswerCallbackQueryAsync(ev.CallbackQuery.Id, "Ok", true);
                            }
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
