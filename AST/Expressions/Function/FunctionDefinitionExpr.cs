using System;
using AST.Statements;
using AST.Visitor;

namespace AST.Expressions.Function
{
    public class FunctionDefinitionExpr : IExpression
    {
        private readonly IdentifierExpr _name;
        private readonly VarDefinitionStmt[] _arguments;
        private readonly IdentifierExpr _returnType;
        private readonly IStatement _body;

        public FunctionDefinitionExpr(IdentifierExpr name, VarDefinitionStmt[] arguments, IStatement body, IdentifierExpr returnType)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            _name = name;

            if (arguments == null)
                throw new ArgumentNullException("arguments");
            _arguments = arguments;

            if(body == null)
                throw new ArgumentNullException("body");
            _body = body;

            if (returnType == null)
                throw new ArgumentNullException("returnType");
            _returnType = returnType;
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public string Name { get { return _name.Name; } }
        public VarDefinitionStmt[] Arguments { get { return _arguments; }}
        public IStatement Body { get { return _body; } }
        public IdentifierExpr ReturnType { get { return _returnType; }}
    }
}
