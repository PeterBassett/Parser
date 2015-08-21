using System;
using AST.Visitor;

namespace AST.Expressions
{
    public class ConditionalExpr : IExpression
    {
        private readonly IExpression _condition;
        private readonly IExpression _thenExpression;
        private readonly IExpression _elseExpression;

        public ConditionalExpr(IExpression condition, IExpression thenExpression, IExpression elseExpression)
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

        public IExpression Condition { get { return _condition; }}
        public IExpression ThenExpression { get { return _thenExpression; } }
        public IExpression ElseExpression { get { return _elseExpression; } }
    }
}
