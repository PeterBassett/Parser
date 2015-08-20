using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;

namespace AST.Visitor
{
    public class PrintVisitor : IExpressionVisitor<string>
    {     
        public string Visit(IdentifierExpr identifierExpr)
        {
            return identifierExpr.Name;
        }

        public string Visit(PlusExpr expr)
        {
            return expr.Left.Accept(this) + "+" + expr.Right.Accept(this);
        }

        public string Visit(ConstantExpr constantExpr)
        {
            return constantExpr.Value.ToString();
        }

        public string Visit(DivExpr expr)
        {
            return expr.Left.Accept(this) + "/" + expr.Right.Accept(this);
        }

        public string Visit(MinusExpr expr)
        {
            return expr.Left.Accept(this) + "-" + expr.Right.Accept(this);
        }

        public string Visit(MultExpr expr)
        {
            return expr.Left.Accept(this) + "*" + expr.Right.Accept(this);
        }

        public string Visit(AssignmentExpr expr)
        {
            return expr.Left.Accept(this) + "=" + expr.Right.Accept(this);
        }

        public string Visit(PowExpr expr)
        {
            return expr.Left.Accept(this) + "^" + expr.Right.Accept(this);
        }

        public string Visit(EqualsExpr expr)
        {
            return expr.Left.Accept(this) + " == " + expr.Right.Accept(this); 
        }

        public string Visit(NotEqualsExpr expr)
        {
            return expr.Left.Accept(this) + " != " + expr.Right.Accept(this);
        }

        public string Visit(GreaterThanExpr expr)
        {
            return expr.Left.Accept(this) + " > " + expr.Right.Accept(this);
        }

        public string Visit(LessThanExpr expr)
        {
            return expr.Left.Accept(this) + " < " + expr.Right.Accept(this);
        }

        public string Visit(GreaterThanOrEqualsExpr expr)
        {
            return expr.Left.Accept(this) + " >= " + expr.Right.Accept(this);
        }

        public string Visit(LessThanOrEqualsExpr expr)
        {
            return expr.Left.Accept(this) + " <= " + expr.Right.Accept(this);
        }


        public string Visit(Expressions.Logical.AndExpr expr)
        {
            throw new System.NotImplementedException();
        }

        public string Visit(Expressions.Logical.OrExpr expr)
        {
            throw new System.NotImplementedException();
        }

        public string Visit(Expressions.Logical.NotExpr expr)
        {
            throw new System.NotImplementedException();
        }
    }
}
