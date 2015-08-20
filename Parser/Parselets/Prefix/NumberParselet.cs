using System.Linq.Expressions;
using AST;
using AST.Expressions.Arithmatic;
using Lexer;

namespace Parser.Parselets.Prefix
{
    internal abstract class NumberParselet : IPrefixParselet
    {
        public IExpression Parse(Parser parser, Token token)
        {
            return new ConstantExpr(Parse(token.Lexeme));
        }

        protected abstract object Parse(string tokenValue);
    }
}
