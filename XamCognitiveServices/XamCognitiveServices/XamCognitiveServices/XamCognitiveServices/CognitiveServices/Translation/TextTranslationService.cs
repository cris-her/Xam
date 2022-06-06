using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CognitiveXamDemo.CognitiveServices;

namespace XamCognitiveServices.CognitiveServices.Translation
{
    public class TextTranslationService : ITextTranslationService
    {
        private IAuthenticationService authenticationService;

        public TextTranslationService
            (IAuthenticationService authService)
        {
            authenticationService = authService;
        }

        string GenerateRequestUri(string endpoint,
            string text, string from, string to)
        {
            string requestUri = endpoint;
            requestUri += $"?text={Uri.EscapeUriString(text)}";
            requestUri += $"&from={from}";
            requestUri += $"&to={to}";
            return requestUri;
        }

        async Task<string> SendRequestAsync(string url, string bearerToken)
        {
            var httpClient =
                new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> TranslateTextAsync(string text, string @from, string to)
        {
            if (string.IsNullOrWhiteSpace(authenticationService.GetAccessToken()))
            {
                await authenticationService.InitializeAsync();
            }
            string requestUri =
                GenerateRequestUri(Constants.TranslatorEndpoint,
                    text,
                    from,
                    to);
            string accessToken =
                authenticationService.GetAccessToken();
            var response =
                await SendRequestAsync(requestUri, accessToken);
            var xml = XDocument.Parse(response);
            return xml.Root.Value;

        }
    }
}
