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

            if (!value.StartsWith("\"") || !value.EndsWith("\""))
                throw new ParseException("String literal found without surrounding quotes.");

            value = value.Substring(1, value.Length - 2);

            return new ConstantExpr(value);
        }
    }
}
