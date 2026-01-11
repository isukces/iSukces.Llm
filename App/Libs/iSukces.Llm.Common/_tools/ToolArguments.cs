using System.Collections.Generic;

namespace iSukces.Llm.Common;

public sealed class ToolArguments
{
    public ToolArguments(JToken? token)
    {
        _token = TokenToObject(token);
    }

    public double GetDouble(string s)
    {
        var value = GetValue(s);
        return value switch
        {
            double number => number,
            long number => number,
            decimal number => (double)number,
            _ => throw new ArgumentException($"Value {s} is not double")
        };
    }

    public object? GetValue(string s)
    {
        var d = ToDictionary();
        //if (_token is null) return null;
        return d[s];
    }

    public Dictionary<string, object?> ToDictionary()
    {
        var dict = new Dictionary<string, object?>();
        foreach (var item in _token)
        {
            dict[item.Key] = item.Value.ToObject<object>();
        }

        return dict;
    }

    private static JObject TokenToObject(JToken? token)
    {
        if (token is null)
            return new JObject();

        if (token is JValue jvalue)
        {
            if (jvalue.Value is string s)
            {
                token = JToken.Parse(s);
            }
        }

        return token.Type switch
        {
            JTokenType.Object => (JObject)token,
            _ => new JObject()
        };
    }

    #region Fields

    private readonly JObject _token;

    #endregion
}
