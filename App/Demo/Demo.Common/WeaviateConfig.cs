using System.Globalization;
using Newtonsoft.Json;

namespace Demo.Common;

public sealed class WeaviateConfig
{
    public static WeaviateConfig GetDemo()
    {
        var config = new WeaviateConfig();
        var file   = typeof(WeaviateConfig).Assembly.FindSlnFolder("Demo\\WeaviateConfig.json");
        if (!File.Exists(file))
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(file, json);
            return config;
        }
        else
        {
            var json = File.ReadAllText(file);
            var tmp  = JsonConvert.DeserializeObject<WeaviateConfig>(json);
            return tmp ?? config;
        }
    }

    public string GetGrpcHost()
    {
        if (string.IsNullOrEmpty(GrpcHost))
            return GetRestHost();
        return GrpcHost;
    }

    public string GetGrpcPort()
    {
        return GrpcPort.ToString(CultureInfo.InvariantCulture);
    }

    public string GetRestHost()
    {
        if (string.IsNullOrEmpty(RestHost))
            return "localhost";
        return RestHost;
    }

    public string GetRestPort()
    {
        return RestPort.ToString(CultureInfo.InvariantCulture);
    }

    public string? RestHost { get; set; } = "localhost";
    public string? GrpcHost { get; set; } = "localhost";
    public int     RestPort { get; set; } = 8080;
    public int     GrpcPort { get; set; } = 50051;

    public bool UseSsl { get; set; }
}
