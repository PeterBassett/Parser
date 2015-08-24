using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AST.Statements
{
    public class AssigmentStmt : IStatement
    {
        private readonly IExpression _lhs;
        private readonly IExpression _rhs;

        public AssigmentStmt(IExpression lhs, IExpression rhs)
        {
            if(lhs == null)
                throw new ArgumentNullException("lhs");
            _lhs = lhs;

            if(rhs == null)
                throw new ArgumentNullException("rhs");            
            _rhs = rhs;
        }
        public T Accept<T, C>(Visitor.IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IExpression Left { get { return _lhs; } }
        public IExpression Right { get { return _rhs; } }
    }
}
