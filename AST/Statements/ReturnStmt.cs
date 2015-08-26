using AST.Statements;
using AST.Visitor;

namespace AST.Expressions.Function
{
    public class ReturnStmt : IStatement
    {
        private readonly IExpression _returnExpr;

        public ReturnStmt(IExpression returnExpr)
        {
            _returnExpr = returnExpr;
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IExpression ReturnExpression { get { return _returnExpr; } }
    }
}
