using AST.Visitor;

namespace AST.Expressions.Arithmatic
{
    public class DivExpr : BinaryOperatorExpr
    {
        public DivExpr(Expression lhs, Expression rhs)
            : base(lhs, rhs)
        {
        }
        
        public override T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
