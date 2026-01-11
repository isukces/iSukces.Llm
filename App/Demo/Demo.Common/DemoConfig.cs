namespace Demo.Common;

public class DemoConfig
{
    public static string ApiUrl { get; set; } = "http://localhost:1234/v1";
    public static string Model { get; set; } = "bielik-11b-v3.0-instruct";

    public static string Sys = "Jesteś asystentem o imieniu Tadeusz. Odpowiadasz krótko i celnie na pytania użytkownika.";
}
