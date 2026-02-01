namespace iSukces.Llm.Common.Schema;

public sealed class JSchema
{
    public JSchema(JObject jObject)
    {
        JObject = jObject;
    }

    public override string ToString()
    {
        return JObject.ToString();
    }

    public JObject JObject { get; }
}
