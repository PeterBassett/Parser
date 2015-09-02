using AST.Visitor;

namespace AST.Statements.Loops
{
    public class WhileStmt : ConditionAndBlockStmt
    {
        public WhileStmt(IExpression condition, IStatement block) : base(condition, block)
        {
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
