using AST.Statements;
using AST.Visitor;

namespace AST.Expressions.Function
{
    public class ReturnStmt : Statement
    {
        private readonly Expression _returnExpr;

        public ReturnStmt(Expression returnExpr)
        {
            _returnExpr = returnExpr;
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public Expression ReturnExpression { get { return _returnExpr; } }
    }
}
