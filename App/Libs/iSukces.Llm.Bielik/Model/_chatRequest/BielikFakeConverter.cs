/*
using iSukces.Llm.Common.Schema;
using Newtonsoft.Json.Converters;

namespace iSukces.Llm.Bielik.Model;

public class ExtraBodyGuidedJsonConverter(ServerType serverType)
    :JsonConverter<BielikExtraBodyGuidedJson>
{
    public override void WriteJson(JsonWriter writer, BielikExtraBodyGuidedJson? value, JsonSerializer serializer)
    {

        var obj = value?.Schema?.JObject;

        if (obj is null)
        {
            writer.WriteNull();
            return;
        }

        var obj2 = new JObject
        {
            ["guided_json"] = obj
        };
        JsonConverter[] x =
        [
            new StringEnumConverter(new SnakeCaseNamingStrategy())
        ];
        obj.WriteTo(writer, x);
    }

    public override BielikExtraBodyGuidedJson? ReadJson(JsonReader reader, Type objectType, BielikExtraBodyGuidedJson? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;
        var obj = JObject.Load(reader);
        return new BielikExtraBodyGuidedJson(new JSchema(obj));
    }
}
*/

