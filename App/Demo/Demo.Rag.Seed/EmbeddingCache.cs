
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Demo.Rag.Seed;

internal sealed class EmbeddingCache
{
    public EmbeddingCache(string fileName)
    {
        var tempPath = Path.GetTempPath();
        filename = Path.Combine(tempPath, fileName);
    }

    public void Save(string chunk, float[] floats)
    {
        var cache = SureLoadd();
        cache[chunk] = floats;

        var json = JsonConvert.SerializeObject(cache, Formatting.Indented);
        File.WriteAllText(filename, json);
    }

    private Dictionary<string, float[]> SureLoadd()
    {
        if (_cache is not null) return _cache;
        if (File.Exists(filename))
        {
            var json = File.ReadAllText(filename);
            _cache = JsonConvert.DeserializeObject<Dictionary<string, float[]>>(json);
        }

        if (_cache is not null) return _cache;
        _cache = new Dictionary<string, float[]>();
        return _cache;
    }

    public bool TryGetValue(string chunk, out float[]? floats)
    {
        var cache = SureLoadd();
        return cache.TryGetValue(chunk, out floats);
    }

    private readonly string filename;

    private Dictionary<string, float[]>? _cache;
}
