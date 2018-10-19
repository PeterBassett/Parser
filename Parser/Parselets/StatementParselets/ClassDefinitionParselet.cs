using System;
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

            var functions = new List<FunctionExpr>();
            var members = new List<VarDefinitionStmt>();

            do
            {
                var statement = parser.ParseNext();

                if (statement is FunctionExpr)
                    functions.Add(statement as FunctionExpr);
                else if (statement is VarDefinitionStmt)
                    members.Add(statement as VarDefinitionStmt);
                else
                    throw new Exception("Unexpected statement type");

            } while (parser.Peek().Type != "RIGHTBRACE");

            parser.Consume("RIGHTBRACE");

            return new ClassDefinitionStmt(new IdentifierExpr(name), members, functions);
        }
    }
}
