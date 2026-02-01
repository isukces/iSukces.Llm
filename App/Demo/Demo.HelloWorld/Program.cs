using Demo.Common;
using iSukces.Llm.Bielik;
using iSukces.Llm.Common;

namespace Demo.HelloWorld;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var protocol = new BielikProtocol();
        var client   = new LlmClient(DemoConfig.ApiUrl, protocol);
        
        await GetDateDemo(DemoConfig.Model, client, protocol);
    }

    private static async Task GetDateDemo(string model, LlmClient client, BielikProtocol protocol)
    {
        var request = new ChatRequest
        {
            Model = model,
            Messages =
            [
                ChatMessage.System("Jesteś ekspertem od literatury języka polskiego."),
                ChatMessage.User("Napisz kilka zdań o Mickiewiczu")
            ]
        };
        
        Console.WriteLine("User:");
        Console.WriteLine(request.Messages[^1].Content);
        
        var obj    = await client.CallCompletions(request);
        var choice = obj.Choices[0];

        if (choice.FinishReason == AiFinishReason.Stop)
        {
            Console.WriteLine(choice.Message.Content);
            return;
        }

        Console.WriteLine("choice.FinishReason=" + choice.FinishReason);
    }
}