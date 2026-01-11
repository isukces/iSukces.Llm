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

    public async Task<LlmChatResponse> CallCompletions(ChatRequest chatRequest)
    {
        var requestJson = _protocol.Serialize(chatRequest, true);
        var content     = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response    = await _httpClient.PostAsync(_callPrefix + "/chat/completions", content);
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
