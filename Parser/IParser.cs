using AST;

namespace Parser
{
    public interface IParser
    {
        IExpression Parse();
        IExpression Parse(int precedence);
    }
}
