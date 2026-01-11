//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;

using Newtonsoft.Json.Converters;

namespace iSukces.Llm.Common;

public static class LlmJsonUtils
{
    public static JsonSerializerSettings CreateSettings(Formatting formatting = Formatting.None)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Converters =
            {
                new StringEnumConverter(new SnakeCaseNamingStrategy())
            },
            NullValueHandling = NullValueHandling.Ignore,
            Formatting        = formatting
        };
        return settings;
    }

    public static T? Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, DefaultSettings);
    }

    public static string Serialize<T>(T message, Formatting formatting = Formatting.None)
    {
        //var    settings = CreateSettings(formatting);
        var settings = formatting == Formatting.None
            ? DefaultSettings
            : IndentedSettings;
        var json = JsonConvert.SerializeObject(message, settings);
        return json;
    }

    public static readonly JsonSerializerSettings DefaultSettings = CreateSettings();
    public static readonly JsonSerializerSettings IndentedSettings = CreateSettings(Formatting.Indented);
}
