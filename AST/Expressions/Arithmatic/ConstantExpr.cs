using AST.Visitor;

namespace AST.Expressions.Arithmatic
{
    public class ConstantExpr : Expression
    {
        private readonly object _value;
        public ConstantExpr(object value)
        {
            _value = value;
        }
        
        public T Accept<T, C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public object Value
        {
            get { return _value; }
        }
    }
}
