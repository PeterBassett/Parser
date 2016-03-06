using System;
using AST.Visitor;

namespace AST.Expressions
{
    public class ConditionalExpr : Expression
    {
        private readonly Expression _condition;
        private readonly Expression _thenExpression;
        private readonly Expression _elseExpression;

        public ConditionalExpr(Expression condition, Expression thenExpression, Expression elseExpression)
        {
            if(condition == null)
                throw new ArgumentNullException("condition");
            _condition = condition;

            if (thenExpression == null)
                throw new ArgumentNullException("thenExpression");
            _thenExpression = thenExpression;

            if (elseExpression == null)
                throw new ArgumentNullException("elseExpression");
            _elseExpression = elseExpression;
        }

        public T Accept<T,C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public Expression Condition { get { return _condition; }}
        public Expression ThenExpression { get { return _thenExpression; } }
        public Expression ElseExpression { get { return _elseExpression; } }
    }
}
