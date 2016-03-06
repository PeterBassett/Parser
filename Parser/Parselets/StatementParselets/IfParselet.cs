using AST.Statements;

namespace Parser.Parselets.StatementParselets
{
    class IfParselet : StatementParselet
    {
        public override Statement Parse(Parser parser, Lexer.Token current)
        {
            parser.Consume("LEFTPAREN");
            var condition = parser.ParseExpression(0);
            parser.Consume("RIGHTPAREN");
            
            var trueBlock = ParseStatement(parser);

            var falseBlock = parser.ConsumeOptional("ELSE") ? ParseStatement(parser) : null;

            return new IfStmt(condition, trueBlock, falseBlock);
        }        
    }
}
