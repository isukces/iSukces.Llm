namespace iSukces.Llm.Common;

public interface ILlmProtocol
{
    LlmChatResponse DeserializeChatResponse(string json);
    string Serialize<T>(T obj, bool humanFriendly = false);
}
