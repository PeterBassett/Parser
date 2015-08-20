
using System;
using System.Text;
using AST.Expressions;
using AST.Expressions.Arithmatic;

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
    }
}
