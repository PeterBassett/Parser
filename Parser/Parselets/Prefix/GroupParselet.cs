using AST;
using Lexer;

namespace Parser.Parselets.Prefix
{
    public class GroupParselet : IPrefixParselet
    {
        public Expression Parse(Parser parser, Token token)
        {
            var expression = parser.ParseNext();
            parser.Consume("RIGHTPAREN");
            return expression;
        }
    }
}
