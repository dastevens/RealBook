using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class SongChart
    {
        public string Raw { get; set; } = string.Empty;
        public string Decoded { get; set; } = string.Empty;
        public string Preprocessed { get; set; } = string.Empty;
        public Token[] Tokens { get; set; } = new Token[0];
    }
}
