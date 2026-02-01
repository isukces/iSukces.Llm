namespace iSukces.Llm.Common.Schema;

[Flags]
public enum JSchemaType
{
    None = 0,
    Null = 1,
    Boolean = 2,
    String = 4,
    Integer = 8,
    Number = 16,
    Object = 32,
    Array = 64,
    Any = String | Number | Integer | Boolean | Object | Array | Null
}


internal static class JSchemaTypeExtensions
{
    public static string ConvertToJsonString(this JSchemaType type)
    {
        return type.ToString().ToLower();
    }
}
