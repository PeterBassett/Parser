using AST.Visitor;

namespace AST.Expressions.Arithmatic
{
    public class MinusExpr : BinaryOperatorExpr
    {
        public MinusExpr(Expression lhs, Expression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
