using System;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;

namespace AST.Visitor
{
    public class EvaluateVisitor : IExpressionVisitor<Value>
    {        
        public Value Visit(IdentifierExpr identifierExpr)
        {            
            return new Value(identifierExpr.Name);
        }

        public Value Visit(PlusExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToNumeric() + expr.Right.Accept(this).ToNumeric());
        }

        public Value Visit(ConstantExpr constantExpr)
        {
            return new Value(new Value(constantExpr.Value));
        }

        public Value Visit(DivExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToNumeric() / expr.Right.Accept(this).ToNumeric());
        }

        public Value Visit(MinusExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToNumeric() - expr.Right.Accept(this).ToNumeric());
        }

        public Value Visit(MultExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToNumeric() * expr.Right.Accept(this).ToNumeric());
        }

        public Value Visit(AssignmentExpr expr)
        {
            return new Value(expr.Right.Accept(this).ToNumeric());
        }

        public Value Visit(PowExpr expr)
        {
            return new Value(Math.Pow((double)expr.Left.Accept(this).ToNumeric(), (double)expr.Right.Accept(this).ToNumeric()));
        }

        public Value Visit(EqualsExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToBoolean() == expr.Right.Accept(this).ToBoolean());
        }

        public Value Visit(NotEqualsExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToBoolean() != expr.Right.Accept(this).ToBoolean());
        }

        public Value Visit(GreaterThanExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToNumeric() > expr.Right.Accept(this).ToNumeric());
        }

        public Value Visit(LessThanExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToNumeric() < expr.Right.Accept(this).ToNumeric());
        }

        public Value Visit(GreaterThanOrEqualsExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToNumeric() >= expr.Right.Accept(this).ToNumeric());
        }

        public Value Visit(LessThanOrEqualsExpr expr)
        {
            return new Value(expr.Left.Accept(this).ToNumeric() <= expr.Right.Accept(this).ToNumeric());
        }


        public Value Visit(Expressions.Logical.AndExpr expr)
        {
            throw new NotImplementedException();
        }

        public Value Visit(Expressions.Logical.OrExpr expr)
        {
            throw new NotImplementedException();
        }

        public Value Visit(Expressions.Logical.NotExpr expr)
        {
            throw new NotImplementedException();
        }
    }
}
