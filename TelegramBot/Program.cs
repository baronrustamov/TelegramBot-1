using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
//using ApiAiSDK;
using ApiAiSDK.Model;

namespace TelegramBot
{
    class Program
    {
        static TelegramBotClient Bot;
        //static ApiAi apiAi;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("");
            //AIConfiguration config = new AIConfiguration("4ef50d63720ce7e9c0dc7a27ecc3e26183dab3a9", SupportedLanguage.Russian); Token агента   https://dialogflow.cloud.google.com
            // apiAi = new ApiAi(config);

            Bot.OnMessage += Bot_OnMessageReceived;
            Bot.OnCallbackQuery += Bot_OnCallbackQueryReceived;

            var me = Bot.GetMeAsync().Result;

            Console.WriteLine(me.FirstName);

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void Bot_OnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал {buttonText}");

            if(buttonText=="Картинка")
            {
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "https://yandex.ru/images/search?pos=2&img_url=https%3A%2F%2Fvk.vkfaces.com%2FOW3VXOkzj_7qMWEnw4SQf3xp4UNxQeO0el5fUw%2FN03e00dUQFw.jpg&text=картинка%20бота&lr=973&rpt=simage&source=wiz&rlt_url=https%3A%2F%2Ftelegra.ph%2Ffile%2F5024f5df44c9829d053dc.jpg&ogl_url=https%3A%2F%2Fvk.vkfaces.com%2FOW3VXOkzj_7qMWEnw4SQf3xp4UNxQeO0el5fUw%2FN03e00dUQFw.jpg");
            }
            else if(buttonText == "Видео")
            {
                await Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "https://www.youtube.com/watch?v=g-WM_w8-UZE");
            }

            await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {buttonText}");
            
        }

        private static async void Bot_OnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text)
                return;
            string name = $"{message.From.FirstName} {message.From.LastName}";

            Console.WriteLine($"{name} отправил сообщение: '{message.Text}'");

            switch(message.Text)
            {
                case "/start":
                    string text =
                      @"Список команд:
                       /start - запуск бота
                       /inline - вывод меню  
                       /keyboard - вывод клавиатуры";
                    await Bot.SendTextMessageAsync(message.From.Id, text);

                    break;
                case "/inline":
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Google","https://www.google.com"),
                            InlineKeyboardButton.WithUrl("MAIL","https://mail.ru")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Картинка"),
                            InlineKeyboardButton.WithCallbackData("Видео")

                        }
                    }); ;
                    await Bot.SendTextMessageAsync(message.From.Id,"Выберите  пункт меню", replyMarkup: inlineKeyboard);
                    break;
                case "/keyboard":
                    var replyKeyboard = new ReplyKeyboardMarkup(new[] {
                        new[]
                        {
                            new KeyboardButton("Привет"),
                            new KeyboardButton("Как дела?")
                        },
                        new[]
                        {
                            new KeyboardButton("Контакт") { RequestContact = true},
                            new KeyboardButton("Геолокация") { RequestLocation = true}
                        }
                        });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сообщение", replyMarkup: replyKeyboard);
                    break;
                default:
                   // var response = apiAi.TextRequest(message.Text);
                   // string answer = response.Result.Fulfillment.Speech;
                   // if (answer == "")
                   //     answer = "Прости, я тебя не понял";
                   // await Bot.SendTextMessageAsync(message.From.Id, answer);
                    break;
            }
        }
    }   
}
