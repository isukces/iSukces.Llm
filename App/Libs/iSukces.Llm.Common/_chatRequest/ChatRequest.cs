using System.Linq;

namespace iSukces.Llm.Common;

public class ChatRequest
{
    public void AppendMessage(ChatMessage message)
    {
        if (Messages.Length == 0)
            Messages = [message];
        else
            Messages = Messages.Append(message).ToArray();
    }

    public void EvaluateToolsCalls(ChatResponseMessage message)
    {
        foreach (var call in message.ToolCalls)
        {
            if (call is ToolsCallFuntion function)
            {
                var func       = Tools.Find(function);
                var funcResult = func.Invoke(new ToolArguments(function.Arguments));
                var m          = function.CreateToolMessage(funcResult);
                AppendMessage(m);
            }
            else
                throw new NotImplementedException();
        }
    }

    public required string        Model    { get; set; }
    public required ChatMessage[] Messages { get; set; }

    public ToolsCollection Tools      { get; set; } = new();
    public ToolChoice      ToolChoice { get; set; }
}
