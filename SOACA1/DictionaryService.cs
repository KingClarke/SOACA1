using System.Collections;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace SOACA1.Services;

public class DictionaryService(HttpClient http)
{
    public async Task<DictionaryEntry?> GetEntryAsync(string word, CancellationToken ct = default)
    {
        var url = $"https://api.dictionaryapi.dev/api/v2/entries/en/{Uri.EscapeDataString(word)}";
        var response = await http.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode) return null;

        var entries = await response.Content.ReadFromJsonAsync<List<DictionaryEntry>>(ct);
        return entries?.FirstOrDefault();
    }
}

public class DictionaryEntry
{
    public string Word { get; set; } = "";
    public string? Phonetic { get; set; }
    public List<Meaning> Meanings { get; set; } = [];
}

public class Meaning
{
    public string PartOfSpeach { get; set; } = "";
    public List<DefinitionItem> Definitions { get; set; } = [];
}

public class DefinitionItem
{
    public string DefinitionText { get; set; } = "";
    public string Example { get; set; }
    public List<string> Synonyms { get; set; } = [];
}
