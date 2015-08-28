using AST;
using AST.Expressions;
using Lexer;

namespace Parser.Parselets.Infix
{
    class ConditionalParselet : IInfixParselet
    {
        public int Precedence { get { return (int)global::Parser.Precedence.Conditional; } }
        public IExpression Parse(Parser parser, IExpression lhs, Token current)
        {
            var thenExpression = parser.ParseNext();

            parser.Consume("COLON");

            var elseExpression = parser.ParseExpression(Precedence - 1);

            return new ConditionalExpr(lhs, thenExpression, elseExpression);
        }
    }
}
