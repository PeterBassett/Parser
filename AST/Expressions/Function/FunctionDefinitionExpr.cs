using System;
using AST.Statements;
using AST.Visitor;

namespace AST.Expressions.Function
{
    public class FunctionDefinitionExpr : FunctionExpr
    {
        private readonly Statement _body;

        public FunctionDefinitionExpr(IdentifierExpr name, VarDefinitionStmt[] arguments, Statement body, IdentifierExpr returnType) 
            : base(name, arguments, returnType)
        {
            if(body == null)
                throw new ArgumentNullException("body");
            _body = body;
        }

        public override T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public Statement Body { get { return _body; } }
    }
}
