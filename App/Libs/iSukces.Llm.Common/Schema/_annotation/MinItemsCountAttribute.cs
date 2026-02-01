namespace iSukces.Llm.Common.Schema;

public sealed class MinItemsCountAttribute : Attribute
{
    public MinItemsCountAttribute(uint minItemsCount)
    {
        MinItemsCount = minItemsCount;
    }

    public uint MinItemsCount { get; }
}
