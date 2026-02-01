namespace iSukces.Llm.Common;

public interface ILlmProtocol
{
    TOut DeserializeChatResponse(string json);
    string Serialize<T>(T obj, bool humanFriendly = false);
}
