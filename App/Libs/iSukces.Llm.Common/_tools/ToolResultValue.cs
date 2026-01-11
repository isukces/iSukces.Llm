namespace iSukces.Llm.Common;

public partial struct ToolResultValue
{
    public static ToolResultValue DateOnly(DateTime now)
    {
        return now.ToString("yyyy-MM-dd");
    }
}
