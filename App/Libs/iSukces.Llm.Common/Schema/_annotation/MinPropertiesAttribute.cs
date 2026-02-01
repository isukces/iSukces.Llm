namespace iSukces.Llm.Common.Schema;

public sealed class MinPropertiesAttribute : Attribute
{
    public MinPropertiesAttribute(uint minProperties)
    {
        MinProperties = minProperties;
    }

    public uint MinProperties { get; }
}