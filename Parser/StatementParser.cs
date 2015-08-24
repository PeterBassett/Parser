using System;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;
using Lexer;
using Parser.Parselets.Infix;
using Parser.Parselets.Prefix;
using Parser.Parselets.Prefix.Statements;

namespace Parser
{
    public class StatementParser : ExpressionParser
    {
        public StatementParser(ILexer lexer)
            : base(lexer)
        {
            RegisterParselet("IDENTIFIER", new IdentifierParselet());
            RegisterParselet("WHILE", new WhileParselet());
            InfixRight<AssignmentExpr>("ASSIGNMENT", Precedence.Assignment);
        }        
    }
}
