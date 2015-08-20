using AST.Visitor;

namespace AST.Expressions.Logical
{
    public class AndExpr : BinaryOperatorExpr
    {
        public AndExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
