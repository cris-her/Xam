using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamCognitiveServices.CognitiveServices.SpellCheck
{
    public interface IBingSpellCheckService
    {
        Task<SpellCheckResult> SpellCheckTextAsync(string text);
    }
}
