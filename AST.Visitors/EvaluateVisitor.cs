using System;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;

namespace AST.Visitor
{
    public class EvaluateVisitor : IExpressionVisitor<Value, Scope>
    {        
        public Value Visit(IdentifierExpr expr, Scope scope)
        {
            return new Value(expr.Name);
        }

        public Value Visit(PlusExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToNumeric() + expr.Right.Accept(this, scope).ToNumeric());
        }

        public Value Visit(ConstantExpr expr, Scope scope)
        {
            return new Value(new Value(expr.Value));
        }

        public Value Visit(DivExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToNumeric() / expr.Right.Accept(this, scope).ToNumeric());
        }

        public Value Visit(MinusExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToNumeric() - expr.Right.Accept(this, scope).ToNumeric());
        }

        public Value Visit(MultExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToNumeric() * expr.Right.Accept(this, scope).ToNumeric());
        }

        public Value Visit(AssignmentExpr expr, Scope scope)
        {
            return new Value(expr.Right.Accept(this, scope).ToNumeric());
        }

        public Value Visit(PowExpr expr, Scope scope)
        {
            return new Value(Math.Pow((double)expr.Left.Accept(this, scope).ToNumeric(), (double)expr.Right.Accept(this, scope).ToNumeric()));
        }

        public Value Visit(EqualsExpr expr, Scope scope)
        {
            //return new Value(expr.Left.Accept(this, scope).ToBoolean() == expr.Right.Accept(this, scope).ToBoolean());
            return new Value(expr.Left.Accept(this, scope).ToObject().Equals(expr.Right.Accept(this, scope).ToObject()));
        }

        public Value Visit(NotEqualsExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToBoolean() != expr.Right.Accept(this, scope).ToBoolean());
        }

        public Value Visit(GreaterThanExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToNumeric() > expr.Right.Accept(this, scope).ToNumeric());
        }

        public Value Visit(LessThanExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToNumeric() < expr.Right.Accept(this, scope).ToNumeric());
        }

        public Value Visit(GreaterThanOrEqualsExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToNumeric() >= expr.Right.Accept(this, scope).ToNumeric());
        }

        public Value Visit(LessThanOrEqualsExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToNumeric() <= expr.Right.Accept(this, scope).ToNumeric());
        }


        public Value Visit(AndExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToBoolean() && expr.Right.Accept(this, scope).ToBoolean());
        }

        public Value Visit(OrExpr expr, Scope scope)
        {
            return new Value(expr.Left.Accept(this, scope).ToBoolean() || expr.Right.Accept(this, scope).ToBoolean());
        }

        public Value Visit(NotExpr expr, Scope scope)
        {
            return new Value(!expr.Right.Accept(this, scope).ToBoolean());
        }

        public Value Visit(ConditionalExpr expr, Scope scope)
        {
            var condition = expr.Condition.Accept(this, scope).ToBoolean();

            if (condition)
                return expr.ThenExpression.Accept(this, scope);
            else
                return expr.ElseExpression.Accept(this, scope);
        }

        public Value Visit(NegationExpr expr, Scope scope)
        {
            return new Value(-expr.Right.Accept(this, scope).ToNumeric());
        }
    }
}
