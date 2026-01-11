using iSukces.Llm.Common;

namespace iSukces.Llm.Bielik.Model;

internal class BielikToolDefinition
{
    public static BielikToolDefinition FromCommonModel(ToolDefinition definition)
    {
        if (definition is ToolDefinitionFunction function)
        {
            return new BielikToolDefinition
            {
                Type     = ToolType.Function,
                Function = ToolFunction.From(function)
            };
        }

        throw new NotImplementedException();
    }

    public ToolType     Type     { get; set; }
    public ToolFunction Function { get; set; }

    public class ToolFunction
    {
        public static ToolFunction From(ToolDefinitionFunction src)
        {
            return new ToolFunction
            {
                Name        = src.Name,
                Description = src.Description,
                Parameters  = BielikParameter.From(src.Parameters),
            };
        }

        public required string Name        { get; set; }
        public required string Description { get; set; }


        public BielikParameter Parameters { get; set; }

        // public LlmToolParameterType Type { get; set; }
    }

    public class BielikParameter
    {
        public static BielikParameter From(LlmToolParameters? src)
        {
            return new BielikParameter
            {
                Required   = src?.Required ?? [],
                Properties = src?.Properties ?? [],
                Type       = LlmToolParameterType.Object
            };
        }

        public LlmToolParameterType Type { get; set; }

        public required Dictionary<string, LlmToolParameterProperty> Properties { get; set; }

        public required string[] Required { get; set; }
    }
}
