using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST.Visitor;

namespace AST.Expressions.Arithmatic
{
    public class MinusExpr : BinaryOperatorExpr
    {
        public MinusExpr(IExpression lhs, IExpression rhs)
            : base(lhs, rhs)
        {
        }
        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
