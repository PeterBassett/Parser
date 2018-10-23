using System;

namespace AST.Statements
{
    public class NullStatement : IBlockStatement
    {
        public T Accept<T, C>(Visitor.IExpressionVisitor<T, C> visitor, C context)
        {
            return default(T);
        }
    }
}
