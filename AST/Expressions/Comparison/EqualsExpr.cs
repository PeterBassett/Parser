using AST.Visitor;

namespace AST.Expressions.Comparison
{
    public class EqualsExpr : BinaryOperatorExpr
    {
        public EqualsExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
