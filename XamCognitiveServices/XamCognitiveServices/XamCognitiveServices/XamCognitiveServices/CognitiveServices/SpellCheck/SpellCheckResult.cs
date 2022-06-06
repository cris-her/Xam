﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamCognitiveServices.CognitiveServices.SpellCheck
{

    public class SpellCheckResult
    {
        public string _type { get; set; }
        public Flaggedtoken[] flaggedTokens { get; set; }
    }

    public class Flaggedtoken
    {
        public int offset { get; set; }
        public string token { get; set; }
        public string type { get; set; }
        public Suggestion[] suggestions { get; set; }
    }

    public class Suggestion
    {
        public string suggestion { get; set; }
        public int score { get; set; }
    }

}
