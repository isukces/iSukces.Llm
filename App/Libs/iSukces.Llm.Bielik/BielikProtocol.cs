using iSukces.Llm.Bielik.Model;
using iSukces.Llm.Common;
using iSukces.Llm.Common.Schema;

namespace iSukces.Llm.Bielik;

public sealed class BielikProtocol : ILlmProtocol
{
    public BielikProtocol(ServerType serverType = ServerType.Unknown)
    {
        ServerType = serverType;
    }

    public ServerType ServerType { get; set; }
    /*private static LlmChatResponseChoiceMessageToolsCallFuntion? Convert(BielikLlmChatResponseChoiceMessageToolsCallFuntion? src)
    {
        if (src is null) return null;
        return new LlmChatResponseChoiceMessageToolsCallFuntion
        {
            Name      = src.Name,
            Arguments = DeserializeToToken(src.Arguments)
        };
    }*/

    private static JToken? DeserializeToToken(string? src)
    {
        if (string.IsNullOrEmpty(src))
            return null;
        return LlmJsonUtils.Deserialize<JToken>(src);
    }

    public TOut DeserializeChatResponse(string json)
    {
        var b = JsonConvert.DeserializeObject<BielikLlmChatResponse>(json, LlmJsonUtils.DefaultSettings);
        if (b is null)
            throw new ArgumentNullException(nameof(b));
        return new TOut
        {
            Created = b.Created,
            Id      = b.Id,
            Model   = b.Model,
            Object  = b.Object,
            Choices = BielikModelConverter.ConvertBielikChatResponseChoices(b.Choices),
            Usage   = b.Usage
        };
    }


    public string Serialize<T>(T obj, bool humanFriendly = false)
    {
        var settings = humanFriendly
            ? LlmJsonUtils.IndentedSettings
            : LlmJsonUtils.DefaultSettings;
        switch (obj)
        {
            case ChatRequest r:
            {
                var proxy = BielikLlmChatRequest.From(r, ServerType);
                return JsonConvert.SerializeObject(proxy, settings);
            }
            case ToolDefinitionFunction f:
            {
                var proxy = BielikToolDefinition.FromCommonModel(f);
                return JsonConvert.SerializeObject(proxy, settings);
            }
            default:
            {
                var json = JsonConvert.SerializeObject(obj, settings);
                return json;
            }
        }
    }

    public JSchemaFeatures GetFeatures()
    {
        if (ServerType == ServerType.LmStudio)
            return JSchemaFeatures.DoNotUseReferences;
        return JSchemaFeatures.Default;
    }
}

public enum ServerType
{
    Unknown,
    LmStudio
}