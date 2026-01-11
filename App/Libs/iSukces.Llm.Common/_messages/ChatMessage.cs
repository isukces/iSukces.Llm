namespace iSukces.Llm.Common;

public class ChatMessage
{
    public required string         Content { get; set; }
    public          MessageRole Role    { get; set; }

    public static ChatMessage System(string content)
    {
        return new ChatMessage
        {
            Content = content,
            Role    = MessageRole.System
        };
    }
    public static ChatMessage User(string content)
    {
        return new ChatMessage
        {
            Content = content,
            Role    = MessageRole.User
        };
    }
}

public sealed class ToolChatMessage : ChatMessage
{
    public ToolChatMessage()
    {
        Role = MessageRole.Tool;
    }

    public ToolsCall Call { get; set; }
}
