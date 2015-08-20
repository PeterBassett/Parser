using AST;
using Lexer;

namespace Parser.Parselets.Prefix
{
    public interface IPrefixParselet
    {
        IExpression Parse(Parser parser, Token current);
    }
}
