using AST.Visitor;

namespace AST.Expressions.Comparison
{
    public class LessThanExpr : BinaryOperatorExpr
    {
        public LessThanExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
