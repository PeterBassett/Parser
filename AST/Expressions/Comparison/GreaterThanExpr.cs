using AST.Visitor;

namespace AST.Expressions.Comparison
{
    public class GreaterThanExpr : BinaryOperatorExpr
    {
        public GreaterThanExpr(Expression lhs, Expression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
