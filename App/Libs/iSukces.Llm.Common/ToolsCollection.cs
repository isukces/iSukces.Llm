using System.Collections.Generic;
using System.Linq;

namespace iSukces.Llm.Common;

public sealed class ToolsCollection
{
    public void Add(ToolDefinition function)
    {
        Tools.Add(function);
    }

    public List<ToolDefinition> Tools { get; } = [];

    public ToolDefinitionFunction Find(ToolsCallFuntion function)
    {
        var tool =  Tools. OfType<ToolDefinitionFunction>()
            .Single(a=>a.Name == function.Name);
        return tool;

    }
}
