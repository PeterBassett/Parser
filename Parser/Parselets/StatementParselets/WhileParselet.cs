using System.Collections.Generic;
using AST;
using AST.Statements;
using AST.Statements.Loops;

namespace Parser.Parselets.Prefix.Statements
{
    class WhileParselet : StatementParser
    {
        public override IStatement Parse(Parser parser, Lexer.Token current)
        {
            parser.Consume("LEFTPAREN");
            var condition = parser.ParseExpression(0);
            parser.Consume("RIGHTPAREN");

            var block = Block(parser, current, "LEFTBRACE", "RIGHTBRACE");

            return new WhileStmt(condition, block);
        }        
    }
}
