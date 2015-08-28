using AST;
using Lexer;

namespace Parser
{
    public interface IParser
    {
        IExpression ParseAll();
        IExpression ParseNext();
        Token Current { get; }    
    }
}
