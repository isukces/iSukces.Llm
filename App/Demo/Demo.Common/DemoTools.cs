using iSukces.Llm.Common;

namespace Demo.Common;

public static class DemoTools
{
    public static ToolDefinitionFunction CubeRoot()
    {
        return new ToolDefinitionFunction
        {
            Name        = "cube_root",
            Description = "Calculates cube root of a number",
            Invoke      = d =>
            {
                var n = d.GetDouble("a");
                return Math.Pow(n, 1.0 / 3.0);
            },
            Parameters = new LlmToolParametersBuilder()
                .WithParameterNumber("a", "Number to calculate cube root of", true)
                .Build()
        };
    }

    public static ToolDefinitionFunction GetDate()
    {
        return new ToolDefinitionFunction
        {
            Name        = "get_current_date",
            Description = "Get current date",
            Invoke      = _ => ToolResultValue.DateOnly(DateTime.Now)
        };
    }
}
