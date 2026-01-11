// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using iSukces.Llm.Common;

namespace iSukces.Llm.Bielik.Model;

public class BielikChatResponseChoice
{
    public ChatResponseChoice ToCommonModel()
    {
        return new ChatResponseChoice
        {
            FinishReason = FinishReason,
            Index        = Index,
            Message      = Message?.ToCommonModel()
        };
    }

    public BielikChatResponseMessage? Message      { get; set; }
    public int                                 Index        { get; set; }
    public AiFinishReason                      FinishReason { get; set; }
}
