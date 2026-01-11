using Demo.Common;
using iSukces.Llm.Bielik;
using iSukces.Llm.Common;

namespace Demo.ListModels;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var protocol = new BielikProtocol();
        var client   = new LlmClient(DemoConfig.ApiUrl, protocol);
        var response = await client.GetModelsAsync();
        
        Console.WriteLine("Available models:");
        foreach (var model in response)
            Console.WriteLine(model.Id);
    }
}