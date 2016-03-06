using System.Collections.Generic;
using AST;
using AST.Expressions;
using AST.Expressions.Function;
using AST.Expressions.Logical;
using AST.Statements;

namespace Parser.Parselets.StatementParselets
{
    class ClassDefinitionParselet : StatementParselet
    {
        public override Statement Parse(Parser parser, Lexer.Token current)
        {
            string name = parser.Current.Lexeme;
            parser.Consume("IDENTIFIER");
            
            parser.Consume("LEFTBRACE");

            var statements = parser.ParseNext();

            parser.Consume("RIGHTBRACE");

            return new ClassDefinitionStmt(new IdentifierExpr(name), statements);
        }
    }
}
