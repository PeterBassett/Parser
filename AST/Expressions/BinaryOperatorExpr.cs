using System;
using AST.Visitor;

namespace AST.Expressions
{
    public abstract class BinaryOperatorExpr : Expression
    {
        private readonly Expression _lhs;
        private readonly Expression _rhs;

        protected BinaryOperatorExpr(Expression lhs, Expression rhs)
        {
            if(lhs == null)
                throw new ArgumentNullException("lhs");
            _lhs = lhs;

            if (rhs == null)
                throw new ArgumentNullException("rhs");
            _rhs = rhs;
        }

        public abstract T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context);
        public Expression Left { get { return _lhs; } }
        public Expression Right { get { return _rhs; } }
    }
}
