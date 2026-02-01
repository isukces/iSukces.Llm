using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iSukces.Llm.Common;

public class LlmClient
{
    public LlmClient(string baseAddress, ILlmProtocol protocol)
    {
        var uri = new Uri(baseAddress);
        _callPrefix = uri.AbsolutePath.TrimEnd('/');
        _protocol   = protocol;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };
    }

    public async Task<TOut> CallCompletions(ChatRequest chatRequest)
    {
        var requestJson = _protocol.Serialize(chatRequest, true);
        var response    = await _httpClient.PostJson(_callPrefix + "/chat/completions", requestJson);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        return _protocol.DeserializeChatResponse(responseJson);
    }

    public async Task<List<LlmModel>> GetModelsAsync()
    {
        var response = await _httpClient.GetAsync(_callPrefix + "/models");
        response.EnsureSuccessStatusCode();
        var responseJson   = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<LlmModelsResponse>(responseJson, LlmJsonUtils.DefaultSettings);
        return result?.Data ?? new List<LlmModel>();
    }

    private readonly ILlmProtocol _protocol;
    private readonly HttpClient _httpClient;
    private readonly string _callPrefix;
}
