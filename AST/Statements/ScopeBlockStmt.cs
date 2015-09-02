using System;
using System.Collections.Generic;
using System.Linq;

namespace AST.Statements
{
    public class ScopeBlockStmt : IBlockStatement
    {
        private readonly IStatement [] _statements;

        public ScopeBlockStmt(IEnumerable<IStatement> statements)
        {
            if(statements == null)
                throw new ArgumentNullException("statements");
            _statements = statements.ToArray();
        }
        public T Accept<T, C>(Visitor.IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IEnumerable<IStatement> Statements { get { return _statements; } }
    }
}
