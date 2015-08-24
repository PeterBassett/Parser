using AST;
using Lexer;

namespace Parser
{
    public interface IParser
    {
        IExpression Parse();
        IExpression Parse(int precedence);
        Token Current { get; }    
    }
}
