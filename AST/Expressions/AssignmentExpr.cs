using System;
using AST.Statements;
using AST.Visitor;

namespace AST.Expressions
{
    public class AssignmentExpr : BinaryOperatorExpr, Statement
    {
       /* private readonly Expression _lhs;
        private readonly Expression _rhs;

        public AssignmentExpr(Expression lhs, Expression rhs)
        {
            if(lhs == null)
                throw new ArgumentNullException("lhs");
            _lhs = lhs;

            if (rhs == null)
                throw new ArgumentNullException("rhs");
            _rhs = rhs;
        }

        public Expression Left { get { return _lhs; } }
        public Expression Right { get { return _rhs; } }

        public T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }*/

        
        public AssignmentExpr(Expression lhs, Expression rhs)
            : base(lhs, rhs)
        {
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }    
    }
}
