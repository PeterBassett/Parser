﻿using AST.Visitor;

namespace AST.Expressions.Logical
{
    public class NotExpr : UnaryOperatorExpr
    {
        public NotExpr(IExpression rhs)
            : base(rhs)
        {
        }

        public override T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
