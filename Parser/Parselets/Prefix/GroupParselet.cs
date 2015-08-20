using AST;
using Lexer;

namespace Parser.Parselets.Prefix
{
    public class GroupParselet : IPrefixParselet
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var expression = parser.Parse();
            parser.Consume("RIGHTPAREN");
            return expression;
        }
    }
}
