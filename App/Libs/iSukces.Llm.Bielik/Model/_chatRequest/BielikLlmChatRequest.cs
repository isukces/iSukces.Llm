using System.Runtime.InteropServices.JavaScript;
using iSukces.Llm.Common;

namespace iSukces.Llm.Bielik.Model;

internal sealed class BielikLlmChatRequest
{
    public static BielikLlmChatRequest From(ChatRequest chatRequest, ServerType serverType)
    {
        var a=  new BielikLlmChatRequest
        {
            Messages    = BielikModelConverter.ConvertCommon(chatRequest.Messages, BielikChatMessage.From),
            Tools       = BielikModelConverter.ConvertTools(chatRequest.Tools.Tools),
            ToolChoice  = chatRequest.ToolChoice,
            Model       = chatRequest.Model,
            Temperature = chatRequest.Temperature
        };

        if (serverType == ServerType.LmStudio)
        {
            a.ResponseFormat = ConvertExtraBody(chatRequest.ExtraBody);
        }
        
        return a;
    }

    private static object? ConvertExtraBody(IExtraBody? src)
    {
        switch (src)
        {
            case null: 
                return null;
            case ExtraBodyGuidedJson a:
            {
                // see https://lmstudio.ai/docs/developer/openai-compat/structured-output?utm_source=chatgpt.com
                var schema = new JObject
                {
                    ["name"]        = "my_schema",
                    ["strict"] = true,
                    ["schema"] = a.Schema.JObject
                };
                var j = new JObject
                {
                    ["type"]        = "json_schema",
                    ["json_schema"] = schema
                };
                return j;
            }
            default:
                throw new NotImplementedException();
        }
    }
   


    public bool ShouldSerializeTemperature()
    {
        return Temperature.HasValue;
    }

    public bool ShouldSerializeToolChoice()
    {
        return ToolChoice != ToolChoice.Empty &&
               ShouldSerializeTools();
    }


    public bool ShouldSerializeTools()
    {
        return Tools != null && Tools.Length > 0;
    }


    public required string                  Model       { get; set; }
    public          BielikChatMessage[]     Messages    { get; set; } = [];
    public          BielikToolDefinition[]? Tools       { get; set; }
    
    //public          IExtraBody?             GuidedJson   { get; set; }
    
    
    /// <summary>
    /// For LM Studio
    /// </summary>
    public object? ResponseFormat { get; set; }
    
    public          double?                 Temperature { get; set; }

    public ToolChoice ToolChoice
    {
        get
        {
            if (ShouldSerializeTools())
                if (field == ToolChoice.Empty)
                    return ToolChoice.Auto;
            return field;
        }
        set;
    }
}

internal sealed class BielikChatMessage : ChatMessage
{
    public static BielikChatMessage From(ChatMessage arg)
    {
        var result = new BielikChatMessage
        {
            Role    = arg.Role,
            Content = arg.Content
        };

        switch (arg)
        {
            case ToolChatMessage tool:
                switch (tool.Call)
                {
                    case ToolsCallFuntion func:
                        result.ToolCallId = func.Id;
                        result.Name       = func.Name;
                        break;
                    default: throw new NotImplementedException();
                }

                break;
            case ChatResponseMessage r:
                result.ToolCalls = BielikModelConverter.ConvertCommon(r.ToolCalls, BielikToolCall.Make);
                break;
            default:
                if (arg.GetType() != typeof(ChatMessage))
                    throw new NotImplementedException(arg.GetType().FullName);
                break;
        }

        return result;
    }

    public string?           ToolCallId { get; set; }
    public string?           Name       { get; set; }
    public BielikToolCall[]? ToolCalls  { get; set; }
}
