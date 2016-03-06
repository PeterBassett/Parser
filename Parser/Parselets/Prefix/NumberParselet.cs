using AST;
using AST.Expressions.Arithmatic;
using Lexer;

namespace Parser.Parselets.Prefix
{
    internal abstract class LiteralParselet : IPrefixParselet
    {
        public Expression Parse(Parser parser, Token token)
        {
            return new ConstantExpr(Parse(token.Lexeme));
        }

        protected abstract object Parse(string tokenValue);
    }
}
