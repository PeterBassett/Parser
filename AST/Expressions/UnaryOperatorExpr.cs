using System;
using AST.Visitor;

namespace AST.Expressions
{
    public abstract class UnaryOperatorExpr : Expression
    {
        private readonly Expression _rhs;

        protected UnaryOperatorExpr(Expression rhs)
        {
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            _rhs = rhs;
        }

        public abstract T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context);
        public Expression Right { get { return _rhs; } }
    }
}
