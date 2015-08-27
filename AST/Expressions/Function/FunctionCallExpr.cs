using System;
using System.Collections.Generic;
using AST.Visitor;

namespace AST.Expressions.Function
{
    public class FunctionCallExpr : IExpression
    {
        private readonly IdentifierExpr _functionName;
        private readonly IExpression[] _arguments;

        public FunctionCallExpr(IdentifierExpr functionName, IExpression[] callSiteArguments)
        {
            if (functionName == null)
                throw new ArgumentNullException("functionName");
            _functionName = functionName;

            if (callSiteArguments == null)
                throw new ArgumentNullException("callSiteArguments");
            _arguments = callSiteArguments;
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IEnumerable<IExpression> Arguments { get { return _arguments; }}
        public IdentifierExpr FunctionName { get { return _functionName; } }

    }
}
