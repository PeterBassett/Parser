using System;
using AST.Visitor;

namespace AST.Statements.Loops
{
    public abstract class ConditionAndBlockStmt : IStatement
    {
        private readonly IExpression _condition;
        private readonly IStatement _block;

        public ConditionAndBlockStmt(IExpression condition, IStatement block)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            _condition = condition;

            if (block == null)
                throw new ArgumentNullException("block");
            _block = block;
        }

        public abstract T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context);

        public IExpression Condition { get { return _condition; } }
        public IStatement Block { get { return _block; } }
    }
}
