using AST;
using Lexer;

namespace Parser.Parselets.Infix
{
    public interface IInfixParselet
    {
        int Precedence { get; }
        IExpression Parse(Parser parser, IExpression lhs, Token current);
    }
}
