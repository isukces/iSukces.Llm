using iSukces.Llm.Common.Schema;

namespace Demo.Structural;

internal static class CtAiSettings
{
    public const SchemaNameStyle NameStyle = SchemaNameStyle.Snake;

    /*public static JsonSerializerOptions SerializationOptions()
    {
        var s= LlmJsonTools.GetSnakeSettings();
        // s.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        return s;
    }*/
}
