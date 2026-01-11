namespace iSukces.Llm.Common;

public class ChatResponseChoice
{
    public ChatResponseMessage? Message      { get; set; }
    public int                  Index        { get; set; }
    public AiFinishReason       FinishReason { get; set; }
}
