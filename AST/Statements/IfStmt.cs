using System;

namespace AST.Statements
{
    public class IfStmt : IBlockStatement
    {
        private readonly Expression _condition;
        private readonly Statement _thenStatement;
        private readonly Statement _elseExpression;

        public IfStmt(Expression condition, Statement thenStatement, Statement elseStatement)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            _condition = condition;

            if (thenStatement == null)
                throw new ArgumentNullException("thenStatement");
            _thenStatement = thenStatement;

            if (elseStatement == null)
                elseStatement = new NoOpStatement();
            _elseExpression = elseStatement;
        }

        public Expression Condition { get { return _condition; } }
        public Statement ThenExpression { get { return _thenStatement; } }
        public Statement ElseExpression { get { return _elseExpression; } }

        public T Accept<T, C>(Visitor.IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
