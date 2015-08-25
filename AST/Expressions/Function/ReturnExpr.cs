using AST.Visitor;

namespace AST.Expressions.Function
{
    public class ReturnExpr : IExpression
    {
        private readonly IExpression _value;
        
        public ReturnExpr(IExpression value)
        {
            _value = value;
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IExpression Value { get { return _value; } }

    }
}
