using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamCognitiveServices.CognitiveServices.Translation
{
    public interface ITextTranslationService
    {
        Task<string> TranslateTextAsync(string text,
            string from, string to);
    }
}
