using AST;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using Lexer;

namespace Parser.Parselets.Prefix
{
    internal class IdentifierParselet : IPrefixParselet
    {
        public IExpression Parse(Parser parser, Token current)
        {            
            return new IdentifierExpr(current.Lexeme);
        }
    }
}
