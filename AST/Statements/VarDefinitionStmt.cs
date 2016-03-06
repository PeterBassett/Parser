using AST.Expressions;
using AST.Visitor;

namespace AST.Statements
{
    public class VarDefinitionStmt : Statement
    {
        private readonly IdentifierExpr _name;
        private readonly IdentifierExpr _type;
        private readonly bool _isConst;
        private readonly Expression _initialValue;

        public VarDefinitionStmt(IdentifierExpr name, IdentifierExpr type, bool isConst, Expression initialValue)
        {
            _name = name;
            _type = type;
            _isConst = isConst;
            _initialValue = initialValue;
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IdentifierExpr Name { get { return _name; } }

        public Expression InitialValue { get { return _initialValue; } }

        public IdentifierExpr Type { get { return _type; } }

        public bool IsConst { get { return _isConst; } }
    }
}
