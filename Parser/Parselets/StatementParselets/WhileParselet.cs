using System;
using AST.Statements;
using AST.Statements.Loops;

namespace Parser.Parselets.StatementParselets
{
    class WhileParselet : StatementParselet
    {
        public override IStatement Parse(Parser parser, Lexer.Token current)
        {
            parser.Consume("LEFTPAREN");
            var condition = parser.ParseExpression(0);
            parser.Consume("RIGHTPAREN");

            var block = ParseStatement(parser);// parser.ParseNext();

            if (!(block is IStatement))
                throw new ParseException("Invalid statement in block.");
            /*
            if (!(block is IBlockStatement))
                parser.Consume("SEMICOLON");
            */
            return new WhileStmt(condition, block as IStatement);
        }
    }
}
