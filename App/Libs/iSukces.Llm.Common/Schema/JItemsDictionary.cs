using System.Collections.Generic;

namespace iSukces.Llm.Common.Schema;

public class JItemsDictionary
{
    public void AddDefinitionReference(string type)
    {
        Items["$ref"] = new JValue("#/definitions/" + type);
    }

    public void AddReference(string? refName)
    {
        Items["$ref"] = new JValue(refName);
    }

    public void AddSchema(JSchema schema)
    {
        Override =  schema.JObject;
    }

    public JObject? Override { get; set; }

    public void AddType(JSchemaType type)
    {
        Items["type"] = type.ConvertToJsonString();
    }

    public JObject? ToJObject()
    {
        if (Override is not null)
            return Override;
        if (Items.Count == 0)
            return null;
        var defsNode = new JObject();
        foreach (var i in Items)
        {
            defsNode[i.Key] = i.Value;
        }

        return defsNode;
    }

    #region Properties

    public int                        Count => Items.Count;
    public Dictionary<string, JToken> Items { get; } = new();

    #endregion
}
