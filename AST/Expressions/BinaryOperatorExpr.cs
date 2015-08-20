using System;
using AST.Visitor;

namespace AST.Expressions
{
    public abstract class BinaryOperatorExpr : IExpression
    {
        private readonly IExpression _lhs;
        private readonly IExpression _rhs;

        protected BinaryOperatorExpr(IExpression lhs, IExpression rhs)
        {
            if(lhs == null)
                throw new ArgumentNullException("lhs");
            _lhs = lhs;

            if (rhs == null)
                throw new ArgumentNullException("rhs");
            _rhs = rhs;
        }

        public abstract T Accept<T>(IExpressionVisitor<T> visitor);
        public IExpression Left { get { return _lhs; } }
        public IExpression Right { get { return _rhs; } }
    }
}
