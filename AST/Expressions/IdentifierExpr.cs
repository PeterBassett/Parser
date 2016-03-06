using AST.Visitor;

namespace AST.Expressions
{
    public class IdentifierExpr : Expression
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

        public T Accept<T, C>(IExpressionVisitor<T,C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
