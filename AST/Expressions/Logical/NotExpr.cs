using AST.Visitor;

namespace AST.Expressions.Logical
{
    public class NotExpr : UnaryOperatorExpr
    {
        public NotExpr(IExpression rhs)
            : base(rhs)
        {
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
