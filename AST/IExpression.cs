using AST.Visitor;

namespace AST
{
    public interface IExpression
    {
        T Accept<T>(IExpressionVisitor<T> visitor);
    }
}
