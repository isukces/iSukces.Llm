using iSukces.Llm.Bielik.Model;
using iSukces.Llm.Common;

namespace iSukces.Llm.Bielik;

internal static class BielikModelConverter
{
    public static ChatResponseChoice[] ConvertBielikChatResponseChoices(BielikChatResponseChoice[]? src)
    {
        return ConvertCommon(src, q =>
        {
            return q.ToCommonModel();
        });
    }

    public static ToolsCall[] ConvertToolsCalls(BielikToolCall[]? src)
    {
        return ConvertCommon(src, a => a.ToCommonModel());
    }


    public static TOut[] ConvertCommon<TIn, TOut>(IReadOnlyCollection<TIn>? src, Func<TIn, TOut> converter)
    {
        if (src is null || src.Count == 0) return [];
        return src.Select(converter).ToArray();
    }

    public static BielikToolDefinition[] ConvertTools(List<ToolDefinition>? src)
    {
        return ConvertCommon(src, BielikToolDefinition.FromCommonModel);
    }
}
