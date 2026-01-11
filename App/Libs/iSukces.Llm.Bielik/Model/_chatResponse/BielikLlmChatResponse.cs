using iSukces.Llm.Common;

namespace iSukces.Llm.Bielik.Model;

internal class BielikLlmChatResponse
{
    public string                     Id      { get; set; } = "";
    public string                     Object  { get; set; } = "";
    public long                       Created { get; set; }
    public string                     Model   { get; set; } = "";
    public BielikChatResponseChoice[] Choices { get; set; } = [];
    public AiUsage                    Usage   { get; set; }
}
