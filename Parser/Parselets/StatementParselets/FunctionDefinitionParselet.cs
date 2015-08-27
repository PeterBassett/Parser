using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.Expando;
using AST;
using AST.Expressions;
using AST.Expressions.Function;
using AST.Statements;

namespace Parser.Parselets.StatementParselets
{
    class FunctionDefinitionParselet : StatementParselet
    {
        public override IStatement Parse(Parser parser, Lexer.Token current)
        {
            string name = null;

            if (parser.Peek().Type == "IDENTIFIER")
            {
                name = parser.Current.Lexeme;
                parser.Consume("IDENTIFIER");
            }

            parser.Consume("LEFTPAREN");
            var parameters = ParseParameterList(parser).ToArray();
            parser.Consume("RIGHTPAREN");

            var token = parser.Peek();

            if (token.Type == "LEFTBRACE")
            {
                var body = parser.Parse();
                return new FunctionDefinitionExpr(new IdentifierExpr(name), parameters, (IStatement)body,
                    new IdentifierExpr("UNKNOWN"));
            }
            else if (token.Type == "RIGHTARROW")
            {
                parser.Consume("RIGHTARROW");
                var body = parser.ParseExpression(0);
                return new LambdaDefinitionExpr(new IdentifierExpr(name), parameters, body,
                    new IdentifierExpr("UNKNOWN"));
            }

            throw new ParseException("Malformed function defintion");
        }

        private IEnumerable<VarDefinitionStmt> ParseParameterList(Parser parser)
        {
            if (parser.Peek().Type == "RIGHTPAREN")
                yield break;

            while (true)
            {
                var name = parser.ParseExpression(0);

                if(!(name is IdentifierExpr))
                    throw new ParseException("Expected parameter name");

                var type = parser.ParseExpression(0);

                if (!(type is IdentifierExpr))
                    throw new ParseException("Expected parameter type");

                yield return new VarDefinitionStmt((IdentifierExpr)name, (IdentifierExpr)type, false, null);

                if (parser.Peek().Type == "RIGHTPAREN")
                    yield break;

                parser.Consume("COMMA");
            }
        }        
    }
}
