using AST.Visitor;

namespace AST.Expressions.Comparison
{
    public class NotEqualsExpr : BinaryOperatorExpr
    {
        public NotEqualsExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
