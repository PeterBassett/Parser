using System;
using AST.Visitor;

namespace AST.Statements.Loops
{
    public abstract class ConditionAndBlockStmt : Statement
    {
        private readonly Expression _condition;
        private readonly Statement _block;

        public ConditionAndBlockStmt(Expression condition, Statement block)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            _condition = condition;

            if (block == null)
                throw new ArgumentNullException("block");
            _block = block;
        }

        public abstract T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context);

        public Expression Condition { get { return _condition; } }
        public Statement Block { get { return _block; } }
    }
}
