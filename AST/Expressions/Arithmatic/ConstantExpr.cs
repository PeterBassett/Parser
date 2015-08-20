using AST.Visitor;

namespace AST.Expressions.Arithmatic
{
    public class ConstantExpr : IExpression
    {
        private readonly object _value;
        public ConstantExpr(object value)
        {
            _value = value;
        }

        public T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public object Value
        {
            get { return _value; }
        }
    }
}
