using iSukces.Llm.Bielik.Model;
using iSukces.Llm.Common;

namespace Bielik.Common.Tests;

public class DeserializeTests
{
    [Fact]
    public void T01_Should_deserialize_LlmChatResponseChoiceMessageToolsCall()
    {
        var json = """
                   {
                     "type": "function",
                     "id": "317702574",
                     "function": {
                       "name": "get_current_date",
                       "arguments": "{}"
                     }
                   }
                   """;
        var obj = LlmJsonUtils.Deserialize<BielikToolCall>(json);
        Assert.Equal(ToolType.Function, obj.Type);
        Assert.Equal("317702574", obj.Id);
        Assert.NotNull(obj.Function);
        Assert.Equal("get_current_date", obj.Function.Name);
        Assert.Equal("{}", obj.Function.Arguments?.ToString());
    }
}
