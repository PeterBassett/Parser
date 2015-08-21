using System;
using AST.Visitor;

namespace AST.Expressions
{
    public abstract class UnaryOperatorExpr : IExpression
    {
        private readonly IExpression _rhs;

        protected UnaryOperatorExpr(IExpression rhs)
        {
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            _rhs = rhs;
        }

        public abstract T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context);
        public IExpression Right { get { return _rhs; } }
    }
}
