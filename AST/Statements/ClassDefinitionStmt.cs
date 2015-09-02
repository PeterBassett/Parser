using AST.Expressions;
using AST.Visitor;

namespace AST.Statements
{
    public class ClassDefinitionStmt : IStatement
    {
        private readonly IdentifierExpr _name;
        private readonly IExpression _statement;

        public ClassDefinitionStmt(IdentifierExpr name, IExpression stmt)
        {
            _name = name;
            _statement = stmt;
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IdentifierExpr Name { get { return _name; } }

        public IExpression InitialValue { get { return _statement; } }
    }
}
