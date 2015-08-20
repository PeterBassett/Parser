namespace Lexer
{
    public interface ILexer
    {
        bool Advance();
        Token Current { get; }    
    }
}
