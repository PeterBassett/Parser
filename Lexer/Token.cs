using System;

namespace Lexer
{
    public struct Token
    {
        public static readonly Token Empty = new Token(true);

        private Token(bool unused) : this()
        {
            Type = "EMPTY";
            Lexeme = null;
            CharNumber = -1;
            LineNumber = -1;
            PositionInStream = -1;
        }

        public Token(string type, string lexeme, int positionInStream, int charNumber, int lineNumber) : this()
        {
            ThrowOnInvalidArguments(type, lexeme, positionInStream, charNumber, lineNumber);

            Type = type;
            Lexeme = lexeme;
            CharNumber = charNumber;
            LineNumber = lineNumber;
            PositionInStream = positionInStream;
        }

        private void ThrowOnInvalidArguments(string type, string token, int positionInStream, int charNumber, int lineNumber)
        {
            if (type == null)
                throw new ArgumentNullException("type", "type can not be null");

            if (type == "")
                throw new ArgumentOutOfRangeException("type", "type can not be empty");

            if(token == null)
                throw new ArgumentNullException("token", "token can not be null");

            if(positionInStream < 0)
                throw new ArgumentOutOfRangeException("positionInStream", "positionInStream must be zero or greater");

            if (charNumber < 0)
                throw new ArgumentOutOfRangeException("charNumber", "charNumber must be zero or greater");

            if (lineNumber < 0)
                throw new ArgumentOutOfRangeException("lineNumber", "lineNumber must be zero or greater");
        }

        public string Type { get; private set; }
        public string Lexeme { get; private set; }
        public int PositionInStream { get; private set; }
        public int CharNumber { get; private set; }
        public int LineNumber { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} : {1} at Line {2}, Column {3}", Type, Lexeme, LineNumber, CharNumber);
        }
    }
}
