using System.Collections.Generic;
using System.Linq;
using AST;
using AST.Expressions;
using AST.Expressions.Function;
using Lexer;

namespace Parser.Parselets.Infix
{
    class FunctionCallParselet : IInfixParselet
    {
        public int Precedence { get { return (int)global::Parser.Precedence.Call; } }
        public Expression Parse(Parser parser, Expression lhs, Token current)
        {
            var functionName = ((IdentifierExpr) lhs);

            // we should already be on the leftparen token.
            var arguments = ParseArguments(parser).ToArray();

            parser.Consume("RIGHTPAREN");
                
            return new FunctionCallExpr(functionName, arguments);
        }

        private IEnumerable<Expression> ParseArguments(Parser parser)
        {
            if (parser.Peek().Type == "RIGHTPAREN")
                yield break;

            while (true)
            {
                yield return parser.ParseExpression(0);

                if (parser.Peek().Type == "RIGHTPAREN")
                    yield break;

                parser.Consume("COMMA");
            }
        }        
    }
}
