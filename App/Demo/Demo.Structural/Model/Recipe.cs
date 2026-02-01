using System.ComponentModel;
using iSukces.Llm.Common.Schema;

namespace Demo.Structural.Model;

public class Recipe
{
    [Description("Nazwa przepisu")]
    [JsonSchemaRequired]
    public string Name { get; set; }

    [Description("Składniki potrawy")]
    [JsonSchemaRequired]
    [MinItemsCount(1)]
    public List<Ingredient> Ingredients { get; set; }

    [Description("Kroki, które należy wykonać")]
    [JsonSchemaRequired]
    public List<string> Steps { get; set; }

    [Description("Poziom trudności")]
    [JsonSchemaRequired]
    public Difficulty Difficulty { get; set; }

    [Description("Czas przygotowania potrawy w minutach")]
    [JsonSchemaRequired]
    public int PreparationTimeMinutes { get; set; }

    [Description("Ilość porcji")]
    [JsonSchemaRequired]
    public int PortionSizePeople { get; set; }
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}
