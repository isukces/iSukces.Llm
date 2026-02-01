using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Common;
using iSukces.Llm.Common.Embeddings;
using Weaviate.Client;

namespace Demo.Rag.Seed;

internal class Program
{
    private static async Task<List<float[]>> CalculateEmbeddings(List<string> chunks, EmbeddingCalculator emb)
    {
        var embeddings = new List<float[]>(chunks.Count);
        var cache      = new EmbeddingCache("ml-workout-demo-embeddings-cache.json");
        var progress   = new ConsoleProgressBar("Obliczanie embeddings");
        progress.Report(0);
        foreach (var chunk in chunks)
        {
            if (!cache.TryGetValue(chunk, out float[] vector))
            {
                vector = await emb.CalculateEmbedding(chunk);
                cache.Save(chunk, vector);
            }

            embeddings.Add(vector);
            progress.Report(embeddings.Count / (double)chunks.Count);
        }

        progress.Complete();
        return embeddings;
    }

    private static IEnumerable<string> ChunkText(string text, int size, int overlap)
    {
        for (var i = 0; i < text.Length; i += size - overlap)
            yield return text.Substring(i, Math.Min(size, text.Length - i));
    }


    private static List<string> LoadTextData(string path)
    {
        return Directory
            .EnumerateFiles(path, "*.md")
            .Select(f => File.ReadAllText(f, Encoding.UTF8))
            .ToList();
    }


    private static async Task Main()
    {
        var slConfig = SilverRetrieverConfig.GetDemo();
        using var emb = new EmbeddingCalculator(slConfig.OpenAiBaseUrl, slConfig.ApiKey)
        {
            EmbeddingModel = slConfig.EmbeddingModel
        };

        var dir = typeof(Program).Assembly.FindCsprojFolder("source-files");
        Console.WriteLine("Przeszujuję " + dir);
        var texts = LoadTextData(dir);

        var chunks = texts
            .SelectMany(t => ChunkText(t, 512, 128))
            .ToList();

        var embeddings = await CalculateEmbeddings(chunks, emb);

        var weaviateConfig = WeaviateConfig.GetDemo();

        using var weaviateClient = await WeaviateClientBuilder.Custom(
                restEndpoint: weaviateConfig.GetRestHost(),
                restPort: weaviateConfig.GetRestPort(),
                grpcEndpoint: weaviateConfig.GetGrpcHost(),
                grpcPort: weaviateConfig.GetGrpcPort(),
                useSsl: weaviateConfig.UseSsl
            )
            .BuildAsync();

        var collection = await Weaviate.CreateCollection(weaviateClient);
        await Weaviate.SaveVectors(chunks, collection, embeddings);

        Console.WriteLine("Koniec");
    }
}