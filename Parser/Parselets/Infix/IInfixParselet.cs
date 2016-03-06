using AST;
using Lexer;

namespace Parser.Parselets.Infix
{
    public interface IInfixParselet
    {
        int Precedence { get; }
        Expression Parse(Parser parser, Expression lhs, Token current);
    }
}
