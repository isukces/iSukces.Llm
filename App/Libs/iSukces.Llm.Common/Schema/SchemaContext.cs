namespace iSukces.Llm.Common.Schema;


public class SchemaContext
{
    public required GetReferenceDelegate TryGetReference { get; init; }
    public required GetSchemaDelegate    TryGetSchema    { get; init; }
    public          SchemaNameStyle      Style           { get; init; }
    public required JSchemaFeatures      Features        { get; init; }
}

public delegate JSchema? GetSchemaDelegate(Type type);
public delegate string? GetReferenceDelegate(Type type);


public static class SchemaContextExtensions
{
    public static bool AllowReferences(this SchemaContext? context)
    {
        var features = context?.Features ?? JSchemaFeatures.Default;
        var allowReferences = (features & JSchemaFeatures.DoNotUseReferences) == 0;
        return allowReferences;
    }
}