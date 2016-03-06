using System;
using System.Collections.Generic;
using AST.Visitor;

namespace AST.Expressions.Function
{
    public class FunctionCallExpr : Expression
    {
        private readonly IdentifierExpr _functionName;
        private readonly Expression[] _arguments;

        public FunctionCallExpr(IdentifierExpr functionName, Expression[] callSiteArguments)
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

        public IEnumerable<Expression> Arguments { get { return _arguments; }}
        public IdentifierExpr FunctionName { get { return _functionName; } }

    }
}
