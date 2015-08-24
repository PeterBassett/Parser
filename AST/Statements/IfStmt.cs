using AST.Expressions;

namespace AST.Statements
{
    public class IfStmt : ConditionalExpr, IStatement
    {
        public IfStmt(IExpression condition, IExpression thenExpression, IExpression elseExpression) :base(condition, thenExpression, elseExpression)
        {            
        }

        public override T Accept<T, C>(Visitor.IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
