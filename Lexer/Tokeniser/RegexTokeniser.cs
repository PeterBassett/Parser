using System;
using System.Text.RegularExpressions;

namespace Lexer.Tokeniser
{
    public class RegexTokeniser
    {
        public RegexTokeniser(string pattern, string type)
            : this(new Regex(pattern, RegexOptions.Compiled), type)
        {
            if (pattern == null)
                throw new ArgumentNullException("pattern");

            if (pattern == "")
                throw new ArgumentOutOfRangeException("pattern", "pattern must be provided");

            if (type == null)
                throw new ArgumentNullException("type");

            if(type == "")
                throw new ArgumentOutOfRangeException("type", "type must be provided");
        }

        public RegexTokeniser(Regex regex, string type)
        {
            Regex = regex;
            Type = type;
        }

        public Regex Regex { get; private set; }
        public string Type { get; private set; }
    }
}
