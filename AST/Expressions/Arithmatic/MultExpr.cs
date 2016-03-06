using AST.Visitor;

namespace AST.Expressions.Arithmatic
{
    public class MultExpr : BinaryOperatorExpr
    {
        public MultExpr(Expression lhs, Expression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
