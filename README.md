# Github Link

https://github.com/KingClarke/SOACA1
Included on the website using the About button.

# SOACA1

A small Blazor Web App built around a rich text editor. While you type in the editor, a
sidebar looks up synonyms and dictionary definitions for the word you enter. Includes a
light/dark mode toggle.

## Requirements

- .NET 10 SDK
- Visual Studio 2022+ (or the `dotnet` CLI)

## Packages
- MudExRichTextEditor

## Run

Open `SOACA1.slnx` in Visual Studio and press F5.

## Project layout

```
SOACA1/
  Program.cs                     App setup and service registration
  DictionaryService.cs           Dictionary API client + response models
  Services/SynonymService.cs     Synonym API client
  Components/
    App.razor                    HTML host page, styles/scripts
    Routes.razor                 Routing + rendering mode
    Layout/
      MainLayout.razor           Page shell, dark mode toggle, sidebar
      SynonymSidebar.razor       Lookup input and results
    Pages/Home.razor             The rich text editor page
  wwwroot/app.css                Global styles (editor card)
SOACA1Tests/                     NUnit test project
```

## Key code

### Program.cs

Registers Blazor Interactive Server, MudBlazor, and a typed `HttpClient` for each API
service. `AddHttpClient<T>()` lets each service receive a ready-to-use `HttpClient` via
constructor injection.

### Components/Pages/Home.razor

Hosts the `MudExRichTextEdit` component and keeps a reference to it with `@ref`. The
"Read only" checkbox is bound to `_readOnly`, which is passed to the editor.

### Components/Layout/SynonymSidebar.razor

- The input uses `@bind:event="oninput"` and `@bind:after="OnInput"`, so the search runs on
  every keystroke.
- The previous request is cancelled with a
  `CancellationTokenSource`, so only the latest word is looked up.
- Calls `SynonymService.GetSynonymsAsync` and `DictionaryService.GetEntryAsync`, then
  renders synonyms as items and up to three definitions.
- Empty input clears the results, `OperationCanceledException` is expected and ignored.

### Services/SynonymService.cs (Datamuse)

- Builds the request URL.
- Checks `IsSuccessStatusCode` and returns an empty list on error instead of throwing.
- Catches `JsonException` for malformed responses.
- Maps the API's `word` field into a `List<string>`.

### DictionaryService.cs (Free Dictionary API)

- Returns `null` when the API responds with an error such as 404 (unknown word).
- Deserializes the JSON into `DictionaryEntry` -> `Meaning` -> `DefinitionItem`.
- JSON property matching is case-insensitive, so `partOfSpeech` maps to `PartOfSpeech`.

### Components/Layout/MainLayout.razor

- `MudThemeProvider` with `@bind-IsDarkMode` toggles the MudBlazor theme.
- The `.dark-shell` CSS class is applied to the page in dark mode for the custom layout
  colours.
- The sidebar is rendered next to `<main>` inside `.page`.

### Styling

- `wwwroot/app.css` holds global styles such as the `.editor-card` wrapper.
- Component-specific styles live in `.razor.css` files next to their components.
- Colours use MudBlazor CSS variables (`--mud-palette-*`) so they follow light/dark mode.

## Tests

`SOACA1Tests` uses NUnit. `FakeHttpMessageHandler` returns canned HTTP responses without
calling the real APIs, so that API failures can be tested without trying to find 1 out of 100,000 words that might be broken.

Covered:

- Synonyms are parsed into a list.
- The searched word is URL-encoded.
- A non-success response returns an empty list.
- A successful dictionary response is mapped into the models.
- A 404 dictionary response returns `null`.

Run with:

Use the `Run All Tests` option in Visual Studio and press F5.

## Sources

- Datamuse API - https://www.datamuse.com/api/
- Free Dictionary API - https://dictionaryapi.dev/
- MudBlazor - https://mudblazor.com/
- MudExRichTextEditor - https://www.nuget.org/packages/MudExRichTextEditor
- Quill (editor engine) - https://quilljs.com/
