using System.Data.Common;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Visitor;

namespace AST.Statements
{
    public class VarDefinitionStmt : IStatement
    {
        private readonly IdentifierExpr _name;
        private readonly IdentifierExpr _type;
        private readonly bool _isConst;
        private readonly ConstantExpr _initialValue;

        public VarDefinitionStmt(IdentifierExpr name, IdentifierExpr type, bool isConst, ConstantExpr initialValue)
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

        public ConstantExpr InitialValue { get { return _initialValue; } }

        public IdentifierExpr Type { get { return _type; } }

        public bool IsConst { get { return _isConst; } }
    }
}
