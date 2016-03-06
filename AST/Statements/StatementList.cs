using System;
using System.Collections.Generic;
using System.Linq;
using AST.Visitor;

namespace AST.Statements
{
    public class StatementList : IBlockStatement
    {
        private readonly Statement [] _statements;

        public StatementList(IEnumerable<Statement> statements)
        {
            if(statements == null)
                throw new ArgumentNullException("statements");
            _statements = statements.ToArray();
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IEnumerable<Statement> Statements { get { return _statements; } }
    }
}
