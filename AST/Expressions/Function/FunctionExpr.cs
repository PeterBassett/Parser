using System;
using AST.Statements;
using AST.Visitor;

namespace AST.Expressions.Function
{
    public class FunctionExpr : IExpression
    {
        private readonly IStatement[] _arguments;
        private readonly IStatement _body;
        public FunctionExpr(IStatement [] arguments, IStatement body)
        {
            if (arguments == null)
                throw new ArgumentNullException("arguments");
            _arguments = arguments;

            if(body == null)
                throw new ArgumentNullException("body");
            _body = body;
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IStatement[] Arguments { get { return _arguments; }}
        public IStatement Body { get { return _body; } }

    }
}
