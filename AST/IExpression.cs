using AST.Visitor;

namespace AST
{
    public interface IExpression
    {
        T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context);
    }
}
