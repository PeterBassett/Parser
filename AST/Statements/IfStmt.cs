using System;

namespace AST.Statements
{
    public class IfStmt : IBlockStatement
    {
        private readonly IExpression _condition;
        private readonly IStatement _thenStatement;
        private readonly IStatement _elseExpression;

        public IfStmt(IExpression condition, IStatement thenStatement, IStatement elseStatement)
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

        public IExpression Condition { get { return _condition; } }
        public IStatement ThenExpression { get { return _thenStatement; } }
        public IStatement ElseExpression { get { return _elseExpression; } }

        public T Accept<T, C>(Visitor.IExpressionVisitor<T, C> visitor, C context)
        {
            return visitor.Visit(this, context);
        }
    }
}
