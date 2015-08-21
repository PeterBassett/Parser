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
        }

        public override T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
