using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AST.Statements.Loops
{
    public class WhileStmt : IStatement
    {
        private readonly IExpression _condition;
        private readonly IStatement _block;

        public WhileStmt(IExpression condition, IStatement block)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            _condition = condition;

            if (block == null)
                throw new ArgumentNullException("block");
            _block = block;
        }

        public T Accept<T, C>(Visitor.IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IExpression Condition { get { return _condition; } }
        public IStatement Block { get { return _block; } }
    }
}
