namespace Lexer
{
    public enum TokenType
    {
        Invalid = 0,
        LeftParen,      //	A left parenthesis (a '(').
        RightParen,     //	A right parenthesis (a ')').
        LeftBrace,      //	A left brace (a '{').
        RightBrace,     //	A right brace (a '}').
        Colon,
        Comma,
        Int,
        Float,        
        String,
        Comment,
        WhiteSpace,
        EndOfFile       // An end of file token.
    }
}
