using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace iSukces.Llm.Common.Embeddings;

 
public sealed class EmbeddingCalculator : IDisposable
{
    public EmbeddingCalculator(string baseUrl, string apiKey)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<float[]> CalculateEmbedding(string text)
    {
        var payload = new
        {
            model = EmbeddingModel,
            input = "</s>" + text
        };
        var resp = await _httpClient.PostObject("/embeddings", payload);
        var body = await resp.Content.ReadAsStringAsync();
        var j    = JObject.Parse(body);

        return j["data"]![0]!["embedding"]!
            .Select(v => (float)v!)
            .ToArray();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    public required string EmbeddingModel { get; set; }

    private readonly HttpClient _httpClient;
}
