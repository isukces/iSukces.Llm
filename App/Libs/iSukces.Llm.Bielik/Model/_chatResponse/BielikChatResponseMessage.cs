using iSukces.Llm.Common;

namespace iSukces.Llm.Bielik.Model;

public class BielikChatResponseMessage
{
    internal ChatResponseMessage ToCommonModel()
    {
        return new ChatResponseMessage
        {
            Content   = Content,
            Role      = Role,
            ToolCalls = BielikModelConverter.ConvertToolsCalls(ToolCalls)
        };
    }

    public MessageRole   Role      { get; set; }
    public string           Content   { get; set; } = "";
    public BielikToolCall[] ToolCalls { get; set; }
}
