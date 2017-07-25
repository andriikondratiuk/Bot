using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SashaGrayBot
{
    public class ParseJson
    {
        public static async Task<T> ResponseJson<T>(HttpClient host,string url)
        {
            var lastBuild = await host.GetAsync(url);
            lastBuild.EnsureSuccessStatusCode();
            var responseBody = await lastBuild.Content.ReadAsStringAsync();
            var str = JObject.Parse(responseBody);
            return JsonConvert.DeserializeObject<T>(str.ToString());
        }
    }
}
