using AST.Expressions.Function;
using AST.Statements;

namespace Parser.Parselets.StatementParselets
{
    class ReturnStatementParselet : StatementParselet
    {
        public override IStatement Parse(Parser parser, Lexer.Token current)
        {
            var expr = parser.ParseExpression(0);

            return new ReturnStmt(expr);
        }        
    }
}
