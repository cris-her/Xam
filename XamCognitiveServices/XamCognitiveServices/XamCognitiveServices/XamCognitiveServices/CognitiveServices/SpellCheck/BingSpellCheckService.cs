using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XamCognitiveServices.CognitiveServices.SpellCheck
{
    public class BingSpellCheckService : IBingSpellCheckService
    {
        public async Task<SpellCheckResult> SpellCheckTextAsync(string text)
        {
            string requestUri =
                GenerateRequestUri(
                    Constants.SpellEndpoint, 
                    text, SpellCheckMode.Spell);
            var response =
                await SendRequestAsync(requestUri, Constants.SpellApiKey);
            var spellCheckResults = 
                JsonConvert
                .DeserializeObject<SpellCheckResult>
                (response);
            return spellCheckResults;
        }

        string GenerateRequestUri(string spellCheckEndpoint, 
            string text, SpellCheckMode mode)
        {
            string requestUri = spellCheckEndpoint;
            requestUri += $"?text={text}";
            requestUri += $"&mode={mode.ToString().ToLower()}";
            return requestUri;

        }

        async Task<string> SendRequestAsync(string url, string apiKey)
        {
            var httpClient = 
                new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

    }
}
