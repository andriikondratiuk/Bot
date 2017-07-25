using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SashaGrayBot.Commands
{
    public class UpdateDev
    {
        public static async Task StartDev(TelegramBotClient client, Telegram.Bot.Types.Message message, string baseAdress, string port)
        {
            using (var host = new HttpClient())
            {
                ClientExtension.PrepareHeader(host);
                var lastBuild = await ParseJson.ResponseJson<JenkisBuildJson>(host, $"http://{baseAdress}:{port}/job/first/lastBuild/api/json");
                var result = await host.GetAsync($"http://{baseAdress}:{port}/job/first/build?token=someAuthorizationFuckingTocketThatICantFindWhereToGenerate");
                JenkisBuildJson resId;
                do
                {
                    Thread.Sleep(1000);
                    resId = await ParseJson.ResponseJson<JenkisBuildJson>(host, $"http://{baseAdress}:{port}/job/first/lastBuild/api/json");
                } while (lastBuild.number == resId.number);
                if (resId.result != null && resId.result.Equals("SUCCESS"))
                {
                    await client.SendTextMessageAsync(message.Chat.Id, $"Готово, билд{resId.displayName}");
                }
                else
                {
                    await client.SendTextMessageAsync(message.Chat.Id, $"Что то пошло не так:(, билд{resId.displayName}");
                }

            }
        }
    }
}
