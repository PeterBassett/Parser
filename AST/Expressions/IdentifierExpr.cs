using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST.Visitor;
using Lexer;

namespace AST.Expressions
{
    public class IdentifierExpr : IExpression
    {
        private readonly string _name;
        public IdentifierExpr(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
