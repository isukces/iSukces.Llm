namespace iSukces.Llm.Common;

public class ChatResponseMessage : ChatMessage
{
    public required ToolsCall[] ToolCalls { get; set; }
}

public abstract class ToolsCall
{
    public string Id { get; set; } = "";
}

public class ToolsCallFuntion : ToolsCall
{

    public ToolChatMessage CreateToolMessage(ToolResultValue content)
    {
        var m = new ToolChatMessage
        {
            Call    = this,
            Content = content.Value
        };
        return m;
    }

    public string  Name      { get; set; } = "";
    public JToken? Arguments { get; set; }
}
