using AST;
using AST.Expressions;
using Lexer;

namespace Parser.Parselets.Prefix
{
    internal class IdentifierParselet : IPrefixParselet
    {
        public Expression Parse(Parser parser, Token current)
        {            
            return new IdentifierExpr(current.Lexeme);
        }
    }
}
