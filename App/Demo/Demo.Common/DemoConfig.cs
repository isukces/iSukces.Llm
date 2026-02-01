using iSukces.Llm.Bielik;

namespace Demo.Common;

public static class DemoConfig
{
    public static string     ApiUrl     { get; set; } = "http://localhost:1234/v1";
    public static string     Model      { get; set; } = "bielik-11b-v3.0-instruct";
    public static ServerType ServerType { get; set; } = ServerType.LmStudio;

    public const string WeaviateCollectionName = "EmailsMLWorkout";

    public static string Sys = "Jesteś asystentem o imieniu Tadeusz. Odpowiadasz krótko i celnie na pytania użytkownika.";
}
