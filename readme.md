# iSukces.Llm

[![NuGet iSukces.Llm.Common](https://img.shields.io/nuget/v/iSukces.Llm.Common?label=iSukces.Llm.Common)](https://www.nuget.org/packages/iSukces.Llm.Common)
[![NuGet iSukces.Llm.Bielik](https://img.shields.io/nuget/v/iSukces.Llm.Bielik?label=iSukces.Llm.Bielik)](https://www.nuget.org/packages/iSukces.Llm.Bielik)

Projekt powstał, aby ułatwić korzystanie z LLM w ekosystemie .NET bez dorzucania do projektow ciężkich zależności.
Celem były lekkie modele danych, prosty klient HTTP i gotowa integracja z Bielikiem.

Zestaw bibliotek .NET 10 (C# 14) do pracy z LLM w stylu OpenAI, wraz z obsługą protokołu Bielik od SpeakLeash.

## Wymagania
- .NET 10 SDK

## Biblioteki
- **iSukces.Llm.Common** (`App/Libs/iSukces.Llm.Common`): wspólne modele zapytań i odpowiedzi (ChatRequest, ChatMessage, ChatResponse), helper `LlmClient` do wywołań HTTP `/chat/completions` i `/models`, oraz wsparcie dla funkcji narzędzi (`ToolDefinitionFunction`, `ToolsCollection`, `ToolArguments`, `ToolResultValue`, `ChatRequest.EvaluateToolsCalls`). Serializacja oparta na Newtonsoft.Json.
- **iSukces.Llm.Bielik** (`App/Libs/iSukces.Llm.Bielik`): implementacja `ILlmProtocol` w klasie `BielikProtocol`, mapująca wspólne modele na specyficzne dla Bielika i odwrotnie (w tym wywołania narzędzi). Zawiera dedykowane klasy modeli Bielika w katalogu `Model`.

## Przykłady (Demo)
Niektóre projekty demo powstały na podstawie filmów z kanału ML-Workout https://www.youtube.com/@ml-workout
- `App/Demo/Demo.ListModels`: pobiera i wypisuje listę modeli z `/models`.
- `App/Demo/Demo.FunctionCall.Date`: pokazuje automatyczne wywołanie funkcji `get_current_date` w trakcie rozmowy i prezentuje odpowiedź agenta.
- `App/Demo/Demo.FunctionCall.QubicSquare`: agent oblicza pierwiastek sześcienny poprzez tool call `cube_root`, a wynik trafia z powrotem do rozmowy.

## Budowanie i testy
- Budowanie: `dotnet build App/iSukces.Llm.sln`
- Testy: `dotnet test App/Tests/Bielik.Common.Tests/Bielik.Common.Tests.csproj`

## Jak uruchomić lokalny Bielik w LM Studio
1. Pobierz LM Studio (https://lmstudio.ai) i zainstaluj.
2. W LM Studio otwórz zakładkę Models → `Download model...`.
3. Wklej adres repozytorium GGUF: `https://huggingface.co/speakleash/Bielik-11B-v3.0-Instruct-GGUF`.
4. Wybierz wariant mieszczący się na karcie RTX 16 GB (np. `Bielik-11B-v3.0-Instruct.Q4_K_M.gguf` lub `Bielik-11B-v3.0-Instruct.Q5_K_M.gguf`) i pobierz.
5. Po pobraniu przejdź do zakładki Server w LM Studio i:
   - wybierz pobrany model,
   - ustaw port nasłuchu na `1234`,
   - włącz serwer (przycisk „Start Server”). Serwis wystawi endpoint zgodny z OpenAI pod `http://localhost:1234/v1`.
6. W pliku `App/Demo/Demo.Common/DemoConfig.cs` dopasuj `ApiUrl` (np. `http://localhost:1234/v1`) i `Model` (np. `bielik-11b-v3.0-instruct`), po czym uruchom dowolne demo z sekcji wyżej.

## Licencja
MIT
