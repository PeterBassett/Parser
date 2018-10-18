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
        private readonly Expression _statement;
        private IdentifierExpr identifierExpr;
        private VarDefinitionStmt [] _members;
        private FunctionDefinitionExpr [] _functions;
        
        public ClassDefinitionStmt(IdentifierExpr name, 
            IEnumerable<VarDefinitionStmt> members, 
            IEnumerable<FunctionDefinitionExpr> functions)
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
        public IEnumerable<FunctionDefinitionExpr> Functions { get { return _functions; } }
    }
}
