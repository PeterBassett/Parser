using AST.Expressions;
using Lexer;
using Parser.Parselets.StatementParselets;

namespace Parser
{
    public class StatementParser : ExpressionParser
    {
        public StatementParser(ILexer lexer)
            : base(lexer)
        {
            RegisterParselet("LEFTBRACE", new BlockParselet("RIGHTBRACE"));
            RegisterParselet("WHILE", new WhileParselet());
            RegisterParselet("IF", new IfParselet());
            InfixRight<AssignmentExpr>("ASSIGNMENT", Precedence.Assignment);
        }        
    }
}
