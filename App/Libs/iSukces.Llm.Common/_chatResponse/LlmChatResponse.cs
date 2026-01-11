namespace iSukces.Llm.Common;

public sealed class LlmChatResponse
{
    public string                  Id      { get; set; } = "";
    public string                  Object  { get; set; } = "";
    public long                    Created { get; set; }
    public string                  Model   { get; set; } = "";
    public ChatResponseChoice[] Choices { get; set; } = [];
    public AiUsage                 Usage   { get; set; }
}
