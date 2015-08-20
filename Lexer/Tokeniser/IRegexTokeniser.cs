using System.Collections.Generic;

namespace Lexer.Tokeniser
{
    public interface IRegexTokeniser
    {
        IEnumerable<RegexTokeniser> Tokenisers { get; }
    }
}
