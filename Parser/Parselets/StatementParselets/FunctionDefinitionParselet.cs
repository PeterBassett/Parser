using System.Collections.Generic;
using AST;
using AST.Expressions;
using AST.Expressions.Function;
using AST.Expressions.Logical;
using AST.Statements;

namespace Parser.Parselets.StatementParselets
{
    class FunctionDefinitionParselet : StatementParselet
    {
        public override Statement Parse(Parser parser, Lexer.Token current)
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
                var body = parser.ParseNext();
                return new FunctionDefinitionExpr(new IdentifierExpr(name), parameters, (Statement)body,
                    new IdentifierExpr("UNKNOWN"));
            }
            else if (token.Type == "RIGHTARROW")
            {
                parser.Consume("RIGHTARROW");
                var body = parser.ParseExpression(0);
                parser.Consume("SEMICOLON");
                return new LambdaDefinitionExpr(new IdentifierExpr(name), parameters, body,
                    new IdentifierExpr("UNKNOWN"));
            }

            throw new ParseException("Malformed function defintion");
        }

        private List<VarDefinitionStmt> ParseParameterList(Parser parser)
        {
            var parameters = new List<VarDefinitionStmt>();

            if (parser.Peek().Type == "RIGHTPAREN")
                return parameters;

            while (true)
            {
                var name = parser.ParseExpression(0);

                if(!(name is IdentifierExpr))
                    throw new ParseException("Expected parameter name");

                Expression type = null;
                if (parser.ConsumeOptional("COLON"))
                {
                    type = parser.ParseExpression(0);

                    if (!(type is IdentifierExpr))
                        throw new ParseException("Expected parameter type");

                }
                else
                {
                    type = new IdentifierExpr("dynamic");
                }

                parameters.Add(new VarDefinitionStmt((IdentifierExpr)name, (IdentifierExpr)type, false, null));

                if (parser.Peek().Type == "RIGHTPAREN")
                    break;

                parser.Consume("COMMA");
            }

            return parameters;
        }        
    }
}
