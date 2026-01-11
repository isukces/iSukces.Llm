using iSukces.Llm.Bielik;
using iSukces.Llm.Bielik.Model;
using iSukces.Llm.Common;

namespace Bielik.Common.Tests;

public class BielikProtocolTests
{
    [Fact]
    public void T01_Should_serialize_LlmToolFunction()
    {
        var protocol = new BielikProtocol();

        var src = new ToolDefinitionFunction
        {
            Description = "Get current date",
            Name        = "get_current_date",
        };

        var json = protocol.Serialize(src, true);
        const string expected = """
                                {
                                  "type": "function",
                                  "function": {
                                    "name": "get_current_date",
                                    "description": "Get current date",
                                    "parameters": {
                                      "type": "object",
                                      "properties": {},
                                      "required": []
                                    }
                                  }
                                }
                                """;
        Assert.Equal(expected, json);
    }


    [Fact]
    public void T02_Should_deserialize_BielikLlmChatResponseChoice()
    {
        const string json = """
                            {
                                "index": 0,
                                "message": {
                                  "role": "assistant",
                                  "content": "\n[TOOL_RESULT]\n{\"date\": \"2023-10-05\"}\n[END_TOOL_RESULT]\nDziś jest 5 października 2023 roku.",
                                  "tool_calls": [
                                    {
                                      "type": "function",
                                      "id": "197311422",
                                      "function": {
                                        "name": "get_current_date",
                                        "arguments": "{}"
                                      }
                                    }
                                  ]
                                },
                                "logprobs": null,
                                "finish_reason": "tool_calls"
                              }
                            """;

        var choice = LlmJsonUtils.Deserialize<BielikChatResponseChoice>(json);
        {
            Assert.NotNull(choice);
            var message = choice.Message;
            Assert.NotNull(message);
            Assert.Equal(MessageRole.Assistant, message.Role);
            Assert.True(message.Content.Length > 10);
            Assert.Single(message.ToolCalls);

            var tool = message.ToolCalls[0];
            Assert.Equal(ToolType.Function, tool.Type);
            Assert.Equal("197311422", tool.Id);
            Assert.NotNull(tool.Function);
            Assert.Equal("get_current_date", tool.Function.Name);
            Assert.Equal("{}", tool.Function.Arguments?.ToString());
        }
        {
            var q       = choice.ToCommonModel();
            var message = q.Message;
            var tool    = message.ToolCalls.Single();
            if (tool is not ToolsCallFuntion f)
                throw new InvalidOperationException();
            Assert.Equal("get_current_date", f.Name);
        }
    }
}
