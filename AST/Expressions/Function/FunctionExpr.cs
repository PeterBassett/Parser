using System;
using AST.Statements;
using AST.Visitor;

namespace AST.Expressions.Function
{
    public abstract class FunctionExpr : IStatement
    {
        private readonly IdentifierExpr _name;
        private readonly VarDefinitionStmt[] _arguments;
        private readonly IdentifierExpr _returnType;

        protected FunctionExpr(IdentifierExpr name, VarDefinitionStmt[] arguments, IdentifierExpr returnType)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            _name = name;

            if (arguments == null)
                throw new ArgumentNullException("arguments");
            _arguments = arguments;

            if (returnType == null)
                throw new ArgumentNullException("returnType");
            _returnType = returnType;
        }

        public abstract T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context);

        public string Name { get { return _name.Name; } }
        public VarDefinitionStmt[] Arguments { get { return _arguments; }}
        public IdentifierExpr ReturnType { get { return _returnType; }}
    }
}
