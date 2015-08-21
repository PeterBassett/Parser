using AST;
using AST.Expressions.Arithmatic;
using Lexer;

namespace Parser.Parselets.Prefix
{
    internal class StringParselet : IPrefixParselet
    {
        public IExpression Parse(Parser parser, Token current)
        {
            var value = current.Lexeme;

            return new ConstantExpr(value);
        }
    }
}
