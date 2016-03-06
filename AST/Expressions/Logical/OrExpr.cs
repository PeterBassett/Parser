using AST.Visitor;

namespace AST.Expressions.Logical
{
    public class OrExpr : BinaryOperatorExpr
    {
        public OrExpr(Expression lhs, Expression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
