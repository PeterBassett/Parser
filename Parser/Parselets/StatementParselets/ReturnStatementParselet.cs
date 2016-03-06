using AST.Expressions.Function;
using AST.Statements;

namespace Parser.Parselets.StatementParselets
{
    class ReturnStatementParselet : StatementParselet
    {
        public override Statement Parse(Parser parser, Lexer.Token current)
        {
            var expr = parser.ParseExpression(0);
            parser.Consume("SEMICOLON");
            return new ReturnStmt(expr);
        }        
    }
}
