using iSukces.Llm.Common;

namespace Bielik.Common.Tests;

public sealed class ToolResultValueTests
{
    [Fact]
    public void T01_Should_convert_from_double()
    {
        ToolResultValue value = 123.44;
        Assert.Equal("123.44", value.Value);

    }
    
    [Fact]
    public void T02_Should_convert_from_decimal()
    {
        ToolResultValue value = 123.44m;
        Assert.Equal("123.44", value.Value);

    }
       
    [Fact]
    public void T03_Should_convert_from_float()
    {
        ToolResultValue value = 123.44f;
        Assert.Equal("123.4400", value.Value[..8]);

    }
    
    
}
