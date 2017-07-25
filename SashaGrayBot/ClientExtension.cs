using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SashaGrayBot
{
    public static class ClientExtension
    {
        private static readonly string jenkinsUserName = BotSettings.Default["UserName"].ToString();
        private static readonly string jenkinsUserToken = BotSettings.Default["PasswordToken"].ToString();
        /// <summary>
        /// Подготовка Header
        /// </summary>
        /// <param name="host">Клиент HttpClient</param>
        public static void PrepareHeaderForJenkins(HttpClient host)
        {
            var credentials = Encoding.ASCII.GetBytes($"{jenkinsUserName}:{jenkinsUserToken}");
            host.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            host.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
        }
    }
}
