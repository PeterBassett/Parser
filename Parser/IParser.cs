using AST;
using Lexer;

namespace Parser
{
    public interface IParser
    {
        Expression ParseAll();
        Expression ParseNext();
        Token Current { get; }    
    }
}
