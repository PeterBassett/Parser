using System;
using AST.Expressions.Arithmatic;
using AST.Visitor;

namespace AST.Expressions
{
    public class AssignmentExpr : BinaryOperatorExpr
    {
        public AssignmentExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
            if (!(lhs is IdentifierExpr))
                throw new ArgumentOutOfRangeException("lhs", "Can only assign to an lValue");
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
