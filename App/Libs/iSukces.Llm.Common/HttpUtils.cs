using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iSukces.Llm.Common;

public static class HttpUtils
{
    extension(HttpClient httpClient)
    {
        public async Task<HttpResponseMessage> PostJson(string url, string json)
        {
            var content  = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            return response;
        }

        public Task<HttpResponseMessage> PostObject<T>(string url, T payload)
        {
            var json = JsonConvert.SerializeObject(payload, Formatting.Indented);
            return httpClient.PostJson(url, json);
        }
    }
}
