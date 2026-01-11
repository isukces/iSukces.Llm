using iSukces.Llm.Common;
using Newtonsoft.Json.Linq;

namespace Bielik.Common.Tests;

public sealed class ToolArgumentsTests
{
    [Fact]
    public void T01_Should_deserialize_empty()
    {
        var args = new ToolArguments(LlmJsonUtils.Deserialize<JToken>("{}"));
        var dict = args.ToDictionary();
        Assert.Empty(dict);
    }

    [Fact]
    public void T02_Should_deserialize_object()
    {
        var args = new ToolArguments(LlmJsonUtils.Deserialize<JToken>(@"{""a"":987654321}"));
        var dict = args.ToDictionary();
        Assert.Single(dict);
        var value = dict["a"];
        Assert.True(value is long);
        Assert.Equal(987654321, (long)value);
    }

    [Fact]
    public void T03_Should_deserialize_Bielik_arguments()
    {
        var args = new ToolArguments(LlmJsonUtils.Deserialize<JToken>(@"""{\""a\"":987654321}"""));
        var dict = args.ToDictionary();
        Assert.Single(dict);
        var value = dict["a"];
        Assert.True(value is long);
        Assert.Equal(987654321, (long)value);
    }

}
