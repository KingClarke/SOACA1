using System.Net.Http.Json;
using System.Text.Json;

namespace SOACA1.Services
{
    public class SynonymService(HttpClient http)
    {
        public async Task<IEnumerable<string>> GetSynonymsAsync(string word, CancellationToken ct = default)
        {
            var url = $"https://api.datamuse.com/words?rel_syn={Uri.EscapeDataString(word)}&max=10";
            var response = await http.GetAsync(url, ct);

            if (!response.IsSuccessStatusCode) return [];
            try
            {
                var words = await response.Content.ReadFromJsonAsync<List<Item>>(ct);
                return words?.Select(w => w.Word).ToList() ?? [];
            }
            catch (JsonException)
            {
                return [];
            }
        }
        private record Item(string Word, int Score);
    }
}