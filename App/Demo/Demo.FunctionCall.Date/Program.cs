using Demo.Common;
using iSukces.Llm.Bielik;
using iSukces.Llm.Common;

namespace Demo.FunctionCall.Date;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Inspirowane filmem https://www.youtube.com/watch?v=wONdGWr8684");
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
                ChatMessage.System(DemoConfig.Sys),
                ChatMessage.User("Jaki dzień był przedwczoraj?")
            ],
            ToolChoice = ToolChoice.Auto
        };
        
        request.Tools.Add(DemoTools.GetDate());
        
        Console.WriteLine("User:");
        Console.WriteLine(request.Messages[^1].Content);
        
        var obj = await client.CallCompletions(request);
        var choice = obj.Choices[0];

        if (choice.FinishReason == AiFinishReason.Stop)
        {
            Console.WriteLine(choice.Message.Content);
            return;
        }

        if (choice.FinishReason == AiFinishReason.ToolsCall)
        {
            choice.Message.Content = "";
            request.AppendMessage(choice.Message);
            request.EvaluateToolsCalls(choice.Message);
            obj = await client.CallCompletions(request);
            choice = obj.Choices[0];
        }

        Console.WriteLine("Agent:");
        Console.WriteLine(choice.Message.Content); 
    }
}