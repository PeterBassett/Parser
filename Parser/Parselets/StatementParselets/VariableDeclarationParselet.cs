﻿using AST;
using AST.Expressions;
using AST.Statements;

namespace Parser.Parselets.StatementParselets
{
    public class VariableDeclarationParselet : StatementParselet
    {
        private readonly bool _constVariables;

        public VariableDeclarationParselet(bool constVariables)
        {
            _constVariables = constVariables;
        }
        
        public override Statement Parse(Parser parser, Lexer.Token current)
        {
            var name = parser.Current.Lexeme;
            
            parser.Consume("IDENTIFIER");

            string type = null;
            if (parser.Peek().Type == "COLON")
                type = ParseTypeSpecified(parser);
        
            Expression initialValue = null;
            if (parser.ConsumeOptional("ASSIGNMENT"))
                initialValue = parser.ParseExpression(0);
            else if (_constVariables)
                throw new ParseException("Const variable declarations must have an initialiser.");
            else if (type == null)
                throw new ParseException("Type must be specified if not assigned at point of definition.");

            parser.Consume("SEMICOLON");

            return new VarDefinitionStmt(new IdentifierExpr(name), new IdentifierExpr(type), _constVariables, initialValue);
        }

        private string ParseTypeSpecified(Parser parser)
        {
            parser.Consume("COLON");

            var type = parser.Current.Lexeme;

            parser.Consume("IDENTIFIER");

            return type;
        }
    }
}
