using AST.Visitor;

namespace AST.Expressions.Arithmatic
{
    public class MultExpr : BinaryOperatorExpr
    {
        public MultExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
