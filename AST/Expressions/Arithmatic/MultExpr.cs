using AST.Visitor;

namespace AST.Expressions.Arithmatic
{
    public class MultExpr : BinaryOperatorExpr
    {
        public MultExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
