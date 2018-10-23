using System;
using AST;
using AST.Expressions;
using Lexer;

namespace Parser.Parselets.Infix
{    
    public class AssignmentOperatorParselet : IInfixParselet
    {
        public int Precedence { get { return (int)global::Parser.Precedence.Assignment; } }

        public Expression Parse(Parser parser, Expression lhs, Token current)
        {
            if (!(lhs is IdentifierExpr))
                throw new ParseException("Expected l-value");

            var right = parser.ParseExpression(Precedence - 1);

            return new AssignmentExpr((IdentifierExpr)lhs, right);
        }
    }
}
