using iSukces.Llm.Common;

namespace iSukces.Llm.Bielik.Model;

public class BielikToolCall
{
    public static BielikToolCall Make(ToolsCall call)
    {
        if (call is ToolsCallFuntion f)
        {
            return new BielikToolCall
            {
                Type     = ToolType.Function,
                Id       = f.Id,
                Function = ToolCallFunction.Make(f)
            };
        }

        throw new NotImplementedException();
    }

    internal ToolsCall ToCommonModel()
    {
        if (Type == ToolType.Function)
        {
            var function = Function;
            if (function is null)
                throw new ArgumentNullException(nameof(function));
            return new ToolsCallFuntion
            {
                Id        = Id,
                Name      = function.Name,
                Arguments = function.Arguments
            };
        }

        throw new NotImplementedException();
    }

    public ToolCallFunction? Function { get; set; }

    public string Id { get; set; } = "";

    public ToolType Type { get; set; }

    public class ToolCallFunction
    {
        public static ToolCallFunction Make(ToolsCallFuntion funtion)
        {
            return new ToolCallFunction
            {
                Name      = funtion.Name,
                Arguments = funtion.Arguments
            };
        }

        public JToken? Arguments { get; set; }

        public string Name { get; set; } = "";
    }
}
