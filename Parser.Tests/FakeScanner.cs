using System;
using Lexer;

namespace Parser.Tests
{
    class FakeScanner : ILexer
    {
        public static readonly Token[] ShortExpression =
        {
            new Token("INTEGER", "1", 0, 0, 0),
            new Token("PLUS", "+", 0, 0, 0),
            new Token("INTEGER", "2", 0, 0, 0),
            new Token("MULT", "*", 0, 0, 0),
            new Token("INTEGER", "3", 0, 0, 0)
        };

        private readonly Token[] _tokens; 
        private int _currentIndex = -1;

        public FakeScanner(Token [] tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException();
            _tokens = tokens;
        }

        public bool Advance()
        {
            _currentIndex++;

            return (_currentIndex < _tokens.Length);
        }

        public Token Current
        {
            get
            {
                if (_currentIndex < 0 || _currentIndex > _tokens.Length - 1)
                    return Token.Empty;

                return _tokens[_currentIndex];
            }
        }
    }
}
