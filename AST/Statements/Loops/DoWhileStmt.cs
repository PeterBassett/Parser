﻿using AST.Visitor;

namespace AST.Statements.Loops
{
    public class DoWhileStmt : ConditionAndBlockStmt
    {
        public DoWhileStmt(Expression condition, Statement block)
            : base(condition, block)
        {
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
