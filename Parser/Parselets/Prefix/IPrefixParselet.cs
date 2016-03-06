using AST;
using Lexer;

namespace Parser.Parselets.Prefix
{
    public interface IPrefixParselet
    {
        Expression Parse(Parser parser, Token current);
    }
}
