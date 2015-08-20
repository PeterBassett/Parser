﻿using AST.Visitor;

namespace AST.Expressions.Arithmatic
{
    public class PlusExpr : BinaryOperatorExpr
    {
        public PlusExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
