using Newtonsoft.Json;

namespace Demo.Common;


public sealed class SilverRetrieverConfig
{
    public static SilverRetrieverConfig GetDemo()
    {
        var config = new SilverRetrieverConfig();
        var file   = typeof(SilverRetrieverConfig).Assembly.FindSlnFolder("Demo\\SilverRetrieverConfig.json");
        if (!File.Exists(file))
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(file, json);
            return config;
        }
        else
        {
            var json = File.ReadAllText(file);
            var tmp  = JsonConvert.DeserializeObject<SilverRetrieverConfig>(json);
            return tmp ?? config;
        }
    }

    public string RestHost       { get; set; } = "localhost";
    public string ApiKey         { get; set; } = "anyKey";
    public int    RestPort       { get; set; } = 8085;
    public string EmbeddingModel { get; set; } = "ipipan/silver-retriever-base-v1";
    public bool   UseSsl         { get; set; } = false;


    [JsonIgnore]
    public string OpenAiBaseUrl => $"http{(UseSsl ? "s" : "")}://{RestHost}:{RestPort}/v1";
}
