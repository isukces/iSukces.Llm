using System.ComponentModel;
using iSukces.Llm.Common.Schema;

namespace Demo.Structural.Model;

public class Ingredient
{
    [Description("Nazwa składnika")]
    [JsonSchemaRequired]
    public string Name { get; set; }

    [Description("Ilość składnika")]
    [JsonSchemaRequired]
    public double Quantity { get; set; }

    [Description("Jednostka miary")]
    [JsonSchemaRequired]
    public string Unit { get; set; }
}
