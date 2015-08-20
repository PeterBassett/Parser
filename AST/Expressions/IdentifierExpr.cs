using AST.Visitor;

namespace AST.Expressions
{
    public class IdentifierExpr : IExpression
    {
        private readonly string _name;
        public IdentifierExpr(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
