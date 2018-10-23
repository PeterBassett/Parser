using System;
using AST.Statements;
using AST.Visitor;

namespace AST.Expressions
{
    public class AssignmentExpr : BinaryOperatorExpr, Statement
    {
        public IdentifierExpr LValueExpression;
        public AssignmentExpr(IdentifierExpr lhs, Expression rhs)
            : base(lhs, rhs)
        {
            LValueExpression = lhs;
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }    
    }
}
