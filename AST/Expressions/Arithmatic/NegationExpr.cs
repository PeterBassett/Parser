using AST.Visitor;

namespace AST.Expressions.Logical
{
    public class NegationExpr : UnaryOperatorExpr
    {
        public NegationExpr(Expression rhs)
            : base(rhs)
        {
        }

        public override T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
