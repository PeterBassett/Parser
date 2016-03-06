using AST.Visitor;

namespace AST.Expressions.Comparison
{
    public class NotEqualsExpr : BinaryOperatorExpr
    {
        public NotEqualsExpr(Expression lhs, Expression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
