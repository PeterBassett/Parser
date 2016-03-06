using AST.Expressions;
using AST.Visitor;

namespace AST.Statements
{
    public class ClassDefinitionStmt : Statement
    {
        private readonly IdentifierExpr _name;
        private readonly Expression _statement;

        public ClassDefinitionStmt(IdentifierExpr name, Expression stmt)
        {
            _name = name;
            _statement = stmt;
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IdentifierExpr Name { get { return _name; } }

        public Expression InitialValue { get { return _statement; } }
    }
}
