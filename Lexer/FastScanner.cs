using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lexer.Tokeniser;

namespace Lexer
{
    public class FastScanner : ILexer
    {
        private static Dictionary<string, string> _keywords = new Dictionary<string, string>()
            {
                {"class", "CLASS"},
                {"true", "BOOLEAN"},
                {"false", "BOOLEAN"},
                {"return", "RETURN"},
                {"val", "VAL"},
                {"var", "VAR"},
            };

        private static Dictionary<string, string> _multiCharacterOperators = new Dictionary<string, string>()
            {
                {"!=", "NOTEQUALS"},
                {"==", "EQUALS"},
                {">=", "GTE"},
                {"<=", "LTE"},
                {"||", "BOOLEAN-OR"},
                {"&&", "BOOLEAN-AND"}
            };

        private static Dictionary<string, string> _singleCharacterOperators = new Dictionary<string, string>()
            {
                {".", "DOT"},
                {"(", "LEFTPAREN"},
                {")", "RIGHTPAREN"},
                {"{", "LEFTBRACE"},
                {"}", "RIGHTBRACE"},
                {"+", "PLUS"},
                {"-", "MINUS"},
                {"*", "MULT"},
                {"/", "DIV"},
                {"%", "MOD"},
                {";", "SEMICOLON"},
                {"?", "QUESTIONMARK"},
                {":", "COLON"},
                {"~", "TILDE"},
                {"^", "POW"},
                {",", "COMMA"},
                {"[", "LEFTSQUAREBRACE"},
                {"]", "RIGHTSQUAREBRACE"},
                {"!", "BOOLEAN-NOT"},
                {"|", "BITWISE-OR"},
                {"&", "BITWISE-AND"},
                {"=", "ASSIGNMENT"},
                {">", "GT"},
                {"<", "LT"}                
            };

        private readonly string _source;
        private int _currentPosition;
        private int _lineNumber = 1;
        private int _charNumber;
        
        public FastScanner(string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
        
            _currentPosition = 0;
            _source = source;

            Current = Token.Empty;
        }

        private char CharAt(int offset) 
        { 
            if(_currentPosition + offset >= _source.Length)
                return '\0';

            return _source[_currentPosition + offset];         
        }

        private char CurrentChar { get { return CharAt(0); } }
        private char PeekChar { get { return CharAt(1); } }
        private bool End { get { return _currentPosition >= _source.Length; } }
        public bool Advance()
        {
            try
            {
                if (End)
                {
                    Current = Token.Empty;
                    return false;
                }

                if (char.IsDigit(CurrentChar))
                    return NumberToken();

                if (CurrentChar == '_' || char.IsLetter(CurrentChar))
                    return IdentifierTokenOrKeyword();

                if (char.IsWhiteSpace(CurrentChar))
                    return WhiteSpaceToken();

                if (CurrentChar == '\"')
                    return QuotedStringToken();

                if (CurrentChar == '\'')
                    return QuotedCharacterToken();

                if (CurrentChar == '/' && PeekChar == '*')
                    return CommentToken();

                if (CurrentChar == '\r' && PeekChar == '\n')
                    _lineNumber++;

                return OperatorToken();
            }
            finally
            {
                if(Current.Lexeme != null)
                    _charNumber += Current.Lexeme.Length;
            }
        }

        private bool OperatorToken()
        {
            if (MultiCharacterOperatorToken())
                return true;

            if (SingleCharacterOperatorToken())
                return true;
                        
            return false;
        }

        private bool MultiCharacterOperatorToken()
        {
            var lexeme = CurrentChar.ToString() + PeekChar.ToString();

            return OperatorToken(_multiCharacterOperators, lexeme);
        }

        private bool SingleCharacterOperatorToken()
        {
            return OperatorToken(_singleCharacterOperators, CurrentChar.ToString());
        }

        private bool OperatorToken(Dictionary<string, string> operators, string lexeme)
        {
            if (operators.ContainsKey(lexeme))
            {
                var tokenName = operators[lexeme];
                Current = new Token(tokenName, lexeme, _currentPosition, _charNumber, _lineNumber);
                _currentPosition += lexeme.Length;
                return true;
            }

            return false;
        }

        private bool IdentifierTokenOrKeyword()
        {
            var identifier = IdentifierToken();

            var lexeme = Current.Lexeme.ToLower();

            if (_keywords.ContainsKey(lexeme))
            {
                var tokenName = _keywords[lexeme];
                Current = new Token(tokenName, lexeme, Current.PositionInStream, Current.CharNumber, Current.LineNumber);
            }

            return true;
        }

        private bool IdentifierToken()
        {
            var start = _currentPosition;

            while (CurrentChar == '_' || char.IsLetterOrDigit(CurrentChar))
                _currentPosition++;

            Current = new Token("IDENTIFIER", _source.Substring(start, _currentPosition - start),
                                    start, _charNumber, _lineNumber);

            return true;
        }

        private bool CommentToken()
        {
            var start = _currentPosition;

            _currentPosition+=2;

            while (!End && CurrentChar != '*' && PeekChar != '/')
                _currentPosition++;

            if (End)
                return false;

            _currentPosition+=2;

            Current = new Token("COMMENT", _source.Substring(start, _currentPosition - start),
                                    start, _charNumber, _lineNumber);

            return true;
        }

        private bool QuotedCharacterToken()
        {
            var start = _currentPosition;

            _currentPosition++;

            if (!char.IsLetterOrDigit(CurrentChar))
                return false;

            _currentPosition++;

            if (CurrentChar != '\'')
                return false;

            _currentPosition++;

            Current = new Token("QUOTED-CHAR", _source.Substring(start, _currentPosition - start),
                                    start, _charNumber, _lineNumber);
            return true;
        }

        private bool QuotedStringToken()
        {
            var start = _currentPosition;

            _currentPosition++;

            while (CurrentChar != '\"')
                _currentPosition++;

            _currentPosition++;

            Current = new Token("QUOTED-STRING", _source.Substring(start, _currentPosition - start),
                                    start, _charNumber, _lineNumber);

            return true;
        }

        private bool WhiteSpaceToken()
        {
            var start = _currentPosition;

            while (char.IsWhiteSpace(CurrentChar))
                _currentPosition++;

            Current = new Token("WHITESPACE", _source.Substring(start, _currentPosition - start),
                                    start, _charNumber, _lineNumber);

            return true;
        }

        private bool NumberToken()
        {
            var tokenType = "INTEGER";
            var start = _currentPosition;

            while(char.IsDigit(CurrentChar))
                _currentPosition++;

            if (CurrentChar == '.')
            {
                _currentPosition++;
                tokenType = "FLOAT";
                while (char.IsDigit(CurrentChar))
                    _currentPosition++;
            }

            Current = new Token(tokenType, _source.Substring(start, _currentPosition - start), 
                                    start, _charNumber, _lineNumber);

            return true;
        }

        public Token Current { get; private set; }
    }
}
