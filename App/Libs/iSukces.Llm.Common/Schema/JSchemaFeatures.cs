namespace iSukces.Llm.Common.Schema;

[Flags]
public enum JSchemaFeatures
{
    Default = 0,
    
    /// <summary>
    /// LM Studio does not support references in JSON schema
    /// </summary>
    DoNotUseReferences = 1
}
