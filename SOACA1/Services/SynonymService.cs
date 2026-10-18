using RtfPipe.Tokens;

namespace SOACA1.Services
{
    public class SynonymService(HttpClient http)
    {
        public async Task<IEnumerable<string>> GetSynonymsAsync(string word, CancellationToken ct = default)
        {
            var url = $"https://api.datamuse.com/words?rel_syn={Uri.EscapeDataString(word)}&max=10";
            var words = await http.GetFromJsonAsync<List<Item>>(url, ct);
            return words?.Select(w => w.Word).ToList() ?? [];
        }
        private record Item(string Word, int score);
    }
}
