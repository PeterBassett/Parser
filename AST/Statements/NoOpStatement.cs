namespace AST.Statements
{
    public class NoOpStatement : IStatement
    {
        public T Accept<T, C>(Visitor.IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
