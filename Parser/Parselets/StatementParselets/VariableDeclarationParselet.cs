using System.Collections.Generic;
using System.Linq;
using AST;
using AST.Expressions;
using AST.Expressions.Function;
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
        
        public override IStatement Parse(Parser parser, Lexer.Token current)
        {
            var name = parser.Current.Lexeme;
            
            parser.Consume("IDENTIFIER");

            string type = null;
            if (parser.Peek().Type == "COLON")
                type = ParseTypeSpecified(parser);
        
            IExpression initialValue = null;
            if (parser.ConsumeOptional("ASSIGNMENT"))
                initialValue = parser.ParseExpression(0);
            else if(_constVariables)
                throw  new ParseException("Const variable declarations must have an initialiser.");
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
