using Demo.Common;
using Demo.Structural.Model;
using iSukces.Llm.Bielik;
using iSukces.Llm.Common;
using iSukces.Llm.Common.Schema;
using Newtonsoft.Json;

namespace Demo.Structural;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        // na podstawie // https://www.youtube.com/watch?v=Q9U3NUgCLt0
        var protocol = new BielikProtocol(DemoConfig.ServerType);
        var client   = new LlmClient(DemoConfig.ApiUrl, protocol);

        var schema = SimpleJsonSchemaBuilder
            .Create<Recipe>()
            .Include<Ingredient>()
            .Build(protocol.GetFeatures());

        var request = new ChatRequest
        {
            Model = DemoConfig.Model,
            Messages =
            [
                ChatMessage.System(SampleData.SystemPrompt),
                ChatMessage.User(SampleData.Krokiety)
            ],
            ExtraBody   = new ExtraBodyGuidedJson(schema),
            Temperature = 0
        };

        var obj    = await client.CallCompletions(request);
        var choice = obj.Choices[0];

        if (choice.FinishReason == AiFinishReason.Stop)
        {
            var content = choice.Message.Content;
            Console.WriteLine(content);
            var recipe        = JsonConvert.DeserializeObject<Recipe>(content, LlmJsonUtils.CreateSettings());
            var wellFormatted = JsonConvert.SerializeObject(recipe, LlmJsonUtils.CreateSettings(Formatting.Indented));
            Console.WriteLine(wellFormatted);

            Console.WriteLine(recipe.Ingredients.Count + " ingredients");
            Console.WriteLine(recipe.Steps.Count + " steps");
            return;
        }

        Console.WriteLine("choice.FinishReason=" + choice.FinishReason);
    }
}
