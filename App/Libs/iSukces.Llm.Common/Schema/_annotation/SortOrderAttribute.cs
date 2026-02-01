namespace iSukces.Llm.Common.Schema;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SortOrderAttribute : Attribute
{
    public SortOrderAttribute(int order)
    {
        Order = order;
    }

    public int Order { get; }
}
