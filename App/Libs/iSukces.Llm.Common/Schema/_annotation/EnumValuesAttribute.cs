namespace iSukces.Llm.Common.Schema;

[AttributeUsage(AttributeTargets.Property)]
public sealed class EnumValuesAttribute : Attribute
{
    public EnumValuesAttribute(params string[] values)
    {
        Values = values;
    }

    public string[] Values { get; }
}
