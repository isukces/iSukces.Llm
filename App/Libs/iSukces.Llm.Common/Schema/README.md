Dostarczamy własny zestaw narzędzi do budowy JSON Schema w sposób elastyczny, tak aby dało się dopasować wynik do wymagań konkretnych hostów LLM. Kluczowy problem, który rozwiązuje, to brak obsługi referencji (`$ref`) w niektórych środowiskach – przykładowo LM Studio oczekuje schematów całkowicie rozwiniętych. Dzięki fladze konfiguracyjnej można więc generować schematy zarówno referencyjne, jak i pozbawione referencji.

# Szybki start
Użyj kodu (typy zdefiniowane w przykładzie `Demo.Structural`)

```cs
var schema = SimpleJsonSchemaBuilder
            .Create<Recipe>()
            .Include<Ingredient>()
            .Build(JSchemaFeatures.DoNotUseReferences);
```
A uzyskasz schema:
```json
{
  "type": "object",
  "properties": {
    "name": {
      "type": "string",
      "description": "Nazwa przepisu"
    },
    "ingredients": {
      "type": "array",
      "description": "Składniki potrawy",
      "minItems": 1,
      "items": {
        "type": "object",
        "properties": {
          "name": {
            "type": "string",
            "description": "Nazwa składnika"
          },
          "quantity": {
            "type": "number",
            "description": "Ilość składnika"
          },
          "unit": {
            "type": "string",
            "description": "Jednostka miary"
          }
        },
        "required": [
          "name",
          "quantity",
          "unit"
        ]
      }
    },
    "steps": {
      "type": "array",
      "description": "Kroki, które należy wykonać",
      "items": {
        "type": "string"
      }
    },
    "difficulty": {
      "type": "string",
      "description": "Poziom trudności",
      "enum": [
        "easy",
        "medium",
        "hard"
      ]
    },
    "preparation_time_minutes": {
      "type": "integer",
      "description": "Czas przygotowania potrawy w minutach"
    },
    "portion_size_people": {
      "type": "integer",
      "description": "Ilość porcji"
    }
  },
  "required": [
    "name",
    "ingredients",
    "steps",
    "difficulty",
    "preparation_time_minutes",
    "portion_size_people"
  ]
}
```

## Dlaczego własny generator?
- Standaryzacja promptów i narzędzi: schemat opisuje wejścia/wyjścia narzędzi dla modeli, więc musi być spójny w całym ekosystemie.
- Różnice implementacyjne hostów LLM: część z nich (LM Studio) nie interpretuje `$ref`. 
- Podczas serializacji/deserializacji możemy wymagać różnych stylów budowania nazw np. camelCase lub snake_case.
- Potrzeba automatyzacji: schemat powinien powstawać na podstawie typów .NET i atrybutów opisujących wymagania (pole obowiązkowe, minimalna liczba elementów, przykłady itp.).

### Atrybuty opisujące schemat
Informacje potrzebne do budowy JSchema są dostarczane przez atrybuty:
- `JsonSchemaRequiredAttribute` – oznacza właściwość jako wymaganą.
- `EnumValuesAttribute` / `ExamplesAttribute` – dostarcza enumerację do `enum` lub listę przykładów (`examples`) w schemacie.
- `MinItemsCountAttribute` / `MinPropertiesAttribute` – ustawia ograniczenia `minItems` oraz `minProperties`.
- `SortOrderAttribute` – pozwala ustalić kolejność właściwości w schemacie.

