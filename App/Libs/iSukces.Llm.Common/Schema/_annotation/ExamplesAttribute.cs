namespace iSukces.Llm.Common.Schema;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ExamplesAttribute : Attribute
{
    public ExamplesAttribute(params object[] values)
    {
        Values = values;
    }

    public object[] Values { get; }
}
