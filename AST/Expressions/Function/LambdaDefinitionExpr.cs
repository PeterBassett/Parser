using System;
using AST.Statements;
using AST.Visitor;

namespace AST.Expressions.Function
{
    public class LambdaDefinitionExpr : FunctionExpr
    {      
        private readonly Expression _body;

        public LambdaDefinitionExpr(IdentifierExpr name, VarDefinitionStmt[] arguments, Expression body, IdentifierExpr returnType)
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

        public Expression Body { get { return _body; } }
    }
}
