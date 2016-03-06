using AST.Visitor;

namespace AST
{
    public interface Expression
    {
        T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context);
    }
}
