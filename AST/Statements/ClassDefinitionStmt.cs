using System.Collections.Generic;
using System.Linq;
using AST.Expressions;
using AST.Expressions.Function;
using AST.Visitor;

namespace AST.Statements
{
    public class ClassDefinitionStmt : Statement
    {
        private readonly IdentifierExpr _name;
        private VarDefinitionStmt [] _members;
        private FunctionExpr [] _functions;
        
        public ClassDefinitionStmt(IdentifierExpr name, 
            IEnumerable<VarDefinitionStmt> members,
            IEnumerable<FunctionExpr> functions)
        {
            _name = name;
            _members = members.ToArray();
            _functions = functions.ToArray();
        }

        public T Accept<T, C>(IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }

        public IdentifierExpr Name { get { return _name; } }
        public IEnumerable<VarDefinitionStmt> Members { get { return _members; } }
        public IEnumerable<FunctionExpr> Functions { get { return _functions; } }
    }
}
