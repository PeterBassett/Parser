using System;
using System.Linq;
using System.Text.RegularExpressions;
using Lexer.Tokeniser;

namespace Lexer
{
    public class Scanner : ILexer
    {
        private readonly string _source;
        private int _currentPosition;
        private int _lineNumber = 1;
        private int _charNumber;
        private readonly IRegexTokeniser _tokeniser;
        private readonly Regex _endOfLineRegex;

        public Scanner(string source, IRegexTokeniser tokeniser)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (tokeniser == null)
                throw new ArgumentNullException("tokeniser");

            _currentPosition = 0;
            _source = source;
            _tokeniser = tokeniser;
            _endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);

            Current = Token.Empty;
        }               

        public bool Advance()
        {
            RegexTokeniser bestTokeniser;
            Match match;

            FindMatchingTokeniser(out bestTokeniser, out match);

            if (bestTokeniser != null)
            {
                Current = new Token(bestTokeniser.Type, match.Value, _currentPosition, _charNumber, _lineNumber);

                _currentPosition += match.Value.Length;

                int charactersAfterLineBreak;
                int linesInMatch;
                if (CurrentTokenSpansLineBreak(out charactersAfterLineBreak, out linesInMatch))
                {
                    _lineNumber += linesInMatch;
                    _charNumber += charactersAfterLineBreak;
                }
                else
                {
                    _charNumber += match.Value.Length;
                }
                
                return true;
            }

            Current = Token.Empty;

            return false;
        }

        private bool CurrentTokenSpansLineBreak(out int characerToLineBreak, out int lines)
        {
            characerToLineBreak = 0;
            lines = 0;

            var matches = _endOfLineRegex.Matches(Current.Lexeme);

            if (matches.Count > 0)
            {
                lines = matches.Count;
                var lastMatch = matches.Cast<Match>().Last();
                characerToLineBreak = Current.Lexeme.Length - lastMatch.Index + lastMatch.Length;
            }

            return matches.Count > 0;
        }

        private void FindMatchingTokeniser(out RegexTokeniser currentBestTokeniser, out Match bestMatch)
        {
            currentBestTokeniser = null;
            bestMatch = null;

            if (_currentPosition >= _source.Length)
                return;

            var currentMaxMatchLength = 0;

            foreach (var tokeniser in _tokeniser.Tokenisers)
            {
                var match = tokeniser.Regex.Match(_source, _currentPosition);

                if (match.Success)
                {
                    if (currentMaxMatchLength < match.Length && match.Index == _currentPosition)
                    {
                        currentMaxMatchLength = match.Length;
                        currentBestTokeniser = tokeniser;
                        bestMatch = match;
                    }
                }
            }
        }

        public Token Current { get; private set; }
    }
}
