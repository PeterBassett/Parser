using AST.Expressions;
using Lexer;
using Parser.Parselets.Infix;
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
            RegisterParselet("FUNCTION", new FunctionDefinitionParselet());
            RegisterParselet("CLASS", new ClassDefinitionParselet());
            RegisterParselet("RETURN", new ReturnStatementParselet());
            RegisterParselet("VAL", new VariableDeclarationParselet(true));
            RegisterParselet("VAR", new VariableDeclarationParselet(false));
            RegisterParselet("LEFTPAREN", new FunctionCallParselet());
            InfixRight<AssignmentExpr>("ASSIGNMENT", Precedence.Assignment);
        }        
    }
}
