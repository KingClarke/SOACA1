using RtfPipe.Tokens;

namespace SOACA1.Services
{
    public class SynonymService(HttpClient http)
    {
        public async Task<string> GetSynonymsAsync(string word, CancellationToken ct = default)
        {
            var response = await http.GetFromJsonAsync($"https://api.datamuse.com/words?rel_syn={word}", ct);
            return words?.Select(w => w.Word).ToList() ?? [];
        }
        private record Item(string Word, int score);
    }
}
