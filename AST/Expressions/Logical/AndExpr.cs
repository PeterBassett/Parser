using AST.Visitor;

namespace AST.Expressions.Logical
{
    public class AndExpr : BinaryOperatorExpr
    {
        public AndExpr(Expression lhs, Expression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
