using System.Collections.Generic;

namespace iSukces.Llm.Common;

public class LlmModel
{
    public string Id { get; set; } = string.Empty;

    public string Object { get; set; } = string.Empty;

    // public long   Created { get; set; }
    public string OwnedBy { get; set; } = string.Empty;
}

public class LlmModelsResponse
{
    public List<LlmModel> Data { get; set; } = new();
}
