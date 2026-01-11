using iSukces.Llm.Common;

namespace iSukces.Llm.Bielik.Model;

internal sealed class BielikLlmChatRequest
{
    public static BielikLlmChatRequest From(ChatRequest chatRequest)
    {
        return new BielikLlmChatRequest
        {
            Messages   = BielikModelConverter.ConvertCommon(chatRequest.Messages, BielikChatMessage.From),
            Tools      = BielikModelConverter.ConvertTools(chatRequest.Tools.Tools),
            ToolChoice = chatRequest.ToolChoice,
            Model      = chatRequest.Model
        };
    }

    public required string                  Model      { get; set; }
    public          BielikChatMessage[]     Messages   { get; set; } = [];
    public          BielikToolDefinition[]? Tools      { get; set; }
    public          ToolChoice              ToolChoice { get; set; }
}

internal sealed class BielikChatMessage : ChatMessage
{
    public static BielikChatMessage From(ChatMessage arg)
    {
        var result = new BielikChatMessage
        {
            Role    = arg.Role,
            Content = arg.Content
        };

        switch (arg)
        {
            case ToolChatMessage tool:
                switch (tool.Call)
                {
                    case ToolsCallFuntion func:
                        result.ToolCallId = func.Id;
                        result.Name       = func.Name;
                        break;
                    default: throw new NotImplementedException();
                }

                break;
            case ChatResponseMessage r:
                result.ToolCalls = BielikModelConverter.ConvertCommon(r.ToolCalls, BielikToolCall.Make);
                break;
            default:
                if (arg.GetType() != typeof(ChatMessage))
                    throw new NotImplementedException(arg.GetType().FullName);
                break;
        }

        return result;
    }

    public string?           ToolCallId { get; set; }
    public string?           Name       { get; set; }
    public BielikToolCall[]? ToolCalls  { get; set; }
}
