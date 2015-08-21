using AST.Visitor;

namespace AST.Expressions.Comparison
{
    public class GreaterThanOrEqualsExpr : BinaryOperatorExpr
    {
        public GreaterThanOrEqualsExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
