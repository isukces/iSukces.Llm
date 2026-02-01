using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace iSukces.Llm.Common.Schema;

public class JsonSchemaBuilder
{
    public void AppendDefinitions(Dictionary<string, JSchema> dict)
    {
        foreach (var i in dict)
            _definitions[i.Key] = i.Value;
    }

    public JSchema Build()
    {
        var jObject = new JObject
        {
            ["type"] = Type.ConvertToJsonString()
        };
        if (!string.IsNullOrEmpty(Description))
            jObject["description"] = Description;

        if (Enum is not null && Enum.Length > 0)
            jObject["enum"] = new JArray(Enum);
        // properties
        if (Properties is not null)
            jObject["properties"] = ToJObject(Properties);
        
        if (MinItems is not null)
            jObject["minItems"] = MinItems;

        // required
        if (Required is not null && Required.Length > 0)
            jObject["required"] = new JArray(Required.Select(a => (object)a));

        // definitions
        if (_definitions.Count > 0 && (Features & JSchemaFeatures.DoNotUseReferences) == 0)
            jObject["definitions"] = ToJObject(_definitions);

        {
            if (Items.Override is not null && (this.Type & JSchemaType.Array) == 0)
            {
                foreach (KeyValuePair<string, JToken?> i in Items.Override)
                    jObject[i.Key] = i.Value;
            }
            else
            {
                var tmp = Items.ToJObject();
                if (tmp is not null)
                    jObject["items"] = tmp;
            }
        }

        return new JSchema(jObject);

        JObject ToJObject(IReadOnlyDictionary<string, JSchema> dictionary)
        {
            var nested = new JObject();
            foreach (var (key, value) in dictionary)
            {
                nested[key] = value.JObject;
            }

            return nested;
        }
    }

    public void Default(int p0)
    {
        throw new NotImplementedException();
    }

    public void Default(double p0)
    {
        throw new NotImplementedException();
    }
 

    public void WithEnum(IEnumerable<string> enumerable)
    {
        Enum = enumerable.ToArray();
        Type = JSchemaType.String;
    }

    public void Examples(IEnumerable<JsonNode?> enumerable)
    {
        throw new NotImplementedException();
    }

    public void MinProperties(uint attrMinProperties)
    {
        throw new NotImplementedException();
    }


    public void Ref(string refName)
    {
        throw new NotImplementedException();
    }

    public JSchemaFeatures Features { get; }

    public string[]? Required { get; set; }

    public IReadOnlyDictionary<string, JSchema>? Properties { get; set; }

    public JItemsDictionary Items { get; } = new();

    public string?     Description { get; set; }
    public JSchemaType Type        { get; set; }

    private readonly OrderedDictionary<string, JSchema> _definitions = new();
    public string[]? Enum { get; set; }

    public JsonSchemaBuilder(JSchemaFeatures features)
    {
        Features = features;
    }

    public uint? MinItems { get; set; }
    
}    
