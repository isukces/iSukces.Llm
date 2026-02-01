using System.Text;
using Demo.Common;
using iSukces.Llm.Bielik;
using iSukces.Llm.Common;
using iSukces.Llm.Common.Embeddings;
using Weaviate.Client;
using Weaviate.Client.Models;

namespace Demo.Rag;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var protocol = new BielikProtocol();
        var client   = new LlmClient(DemoConfig.ApiUrl, protocol);

        //await GetDateDemo(DemoConfig.Model, client, protocol);

        var question = "czym jest ruff?";
        Console.WriteLine("User:");
        Console.WriteLine(question);

        Console.WriteLine("--- Odpowiedź bez RAG");
        await Ask(question, client, []);

        var embedding = await CalculateEmbedding(question);
        var documents = await FindDocuments(embedding);
        Console.WriteLine("--- Odpowiedź z RAG");
        await Ask(question, client, documents);
        return;

        static async Task<float[]> CalculateEmbedding(string question)
        {
            var slConfig = SilverRetrieverConfig.GetDemo();
            using var emb = new EmbeddingCalculator(slConfig.OpenAiBaseUrl, slConfig.ApiKey)
            {
                EmbeddingModel = slConfig.EmbeddingModel
            };
            var embedding = await emb.CalculateEmbedding($"Pytanie:{question}");
            return embedding;
        }

        static async Task<DocumentInfo[]> FindDocuments(float[] embedding)
        {
            var config = WeaviateConfig.GetDemo();
            using var weaviateClient = await WeaviateClientBuilder.Custom(
                    restEndpoint: config.GetRestHost(),
                    restPort: config.GetRestPort(),
                    grpcEndpoint: config.GetGrpcHost(),
                    grpcPort: config.GetGrpcPort(),
                    useSsl: config.UseSsl
                )
                .BuildAsync();
            var collection = weaviateClient.Collections[DemoConfig.WeaviateCollectionName];

            var queryResults = await collection.Query.NearVector(
                embedding,
                limit: 2,
                returnMetadata: new MetadataQuery(MetadataOptions.Distance));

            var documents = queryResults
                .Select(r => new DocumentInfo(r.Properties["text"].ToString(), r.Metadata.Distance ?? double.NaN))
                .ToArray();
            return documents;
        }

        static async Task Ask(string question, LlmClient client, DocumentInfo[] documents)
        {
            if (documents.Length > 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Odpowiedz na pytanie używając poniższych dokumentów:");
                sb.AppendLine($"Pytanie: {question}");
                for (var idx = 0; idx < documents.Length; idx++)
                {
                    var hint = documents[idx];
                    sb.AppendLine($"<dokument {idx + 1}>{hint.Text}</dokument {idx + 1}>");
                }

                question = sb.ToString();
            }

            var request = new ChatRequest
            {
                Model = DemoConfig.Model,
                Messages =
                [
                    ChatMessage.System("Jesteś ekspertem od analizy dokumentacji i wyjaśniania definicji"),
                    ChatMessage.User(question)
                ]
            };

            var obj    = await client.CallCompletions(request);
            var choice = obj.Choices[0];

            if (choice.FinishReason != AiFinishReason.Stop)
                throw new InvalidOperationException("Nieznana przyczyna zakończenia " + choice.FinishReason);
            Console.WriteLine(choice.Message.Content);
        }
    }
}
