using System.Collections.Generic;

namespace iSukces.Llm.Common;

public abstract class ToolDefinition
{
}


public sealed class ToolDefinitionFunction : ToolDefinition
{
    public required string                               Name        { get; set; }
    public required string                               Description { get; set; }
    public          LlmToolParameters?                   Parameters  { get; set; }
    public          Func<ToolArguments, ToolResultValue> Invoke      { get; set; }
}

public class LlmToolParameters
{
    public LlmToolParameterType                         Type       { get; set; } = LlmToolParameterType.Object;
    public Dictionary<string, LlmToolParameterProperty> Properties { get; set; } = [];
    public string[]                                     Required   { get; set; } = [];
}

public class LlmToolParameterProperty
{
    public required LlmToolParameterType Type        { get; set; }
    public required string               Description { get; set; }
}

public class LlmToolParametersBuilder
{
    public LlmToolParametersBuilder WithParameterNumber(string s, string numberToCalculateCubeRootOf, bool b)
    {
        Properties.Add(s, new LlmToolParameterProperty
        {
            Description = numberToCalculateCubeRootOf,
            Type        = LlmToolParameterType.Number
        });
        return this;
    }

    public List<string> Required { get; } = [];
    public Dictionary<string, LlmToolParameterProperty> Properties { get; } = [];

    public LlmToolParameters Build()
    {
        return new LlmToolParameters
        {
            Required   = Required.ToArray(),
            Properties = Properties
        };
    }
}