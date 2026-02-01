using iSukces.Llm.Common.Schema;

namespace iSukces.Llm.Common;

public interface IExtraBody
{
    
}

public sealed class ExtraBodyGuidedJson: IExtraBody
{
    public ExtraBodyGuidedJson(JSchema schema)
    {
        Schema = schema;
    }

    public JSchema Schema { get;  }
}