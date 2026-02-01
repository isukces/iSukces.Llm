using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Common;
using Weaviate.Client;
using Weaviate.Client.Models;

namespace Demo.Rag.Seed;

internal class Weaviate
{
    public static async Task<CollectionClient> CreateCollection(WeaviateClient client)
    {
        var currencTollections = new List<CollectionConfig>();
        await foreach (var x in client.Collections.List())
            currencTollections.Add(x);

        var currentCollection = currencTollections.FirstOrDefault(a => a.Name == DemoConfig.WeaviateCollectionName);
        if (currentCollection is not null)
        {
            await client.Collections.Delete(DemoConfig.WeaviateCollectionName);
        }

        var createParams = new CollectionCreateParams
        {
            Name        = DemoConfig.WeaviateCollectionName,
            Description = "Emails ML Workout",
            Properties =
            [
                new Property
                {
                    Name     = "text",
                    DataType = DataType.Text
                }
            ],
            VectorConfig = Configure.Vector("default", t => t.SelfProvided(),
                index: new VectorIndex.HNSW
                {
                    Distance = VectorIndexConfig.VectorDistance.Cosine
                })
        };

        var collection = await client.Collections.Create(createParams);
        return collection;
    }

    public static async Task SaveVectors(List<string> chunks, CollectionClient collection, List<float[]> embeddings)
    {
        var progress = new ConsoleProgressBar("Uzupełnianie bazy");
        for (var i = 0; i < chunks.Count; i++)
        {
            progress.Report((double)i / chunks.Count);
            await collection.Data.Insert(
                new
                {
                    text = chunks[i]
                },

                vectors: embeddings[i]);
        }

        progress.Complete();
    }
}
