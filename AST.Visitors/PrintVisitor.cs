using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;

namespace AST.Visitor
{
    public class PrintVisitor : IExpressionVisitor<string, Scope>
    {
        public string Visit(IdentifierExpr expr, Scope scope)
        {
            return expr.Name;
        }

        public string Visit(PlusExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "+" + expr.Right.Accept(this, scope);
        }

        public string Visit(ConstantExpr expr, Scope scope)
        {
            return expr.Value.ToString();
        }

        public string Visit(DivExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "/" + expr.Right.Accept(this, scope);
        }

        public string Visit(MinusExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "-" + expr.Right.Accept(this, scope);
        }

        public string Visit(MultExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "*" + expr.Right.Accept(this, scope);
        }

        public string Visit(AssignmentExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "=" + expr.Right.Accept(this, scope);
        }

        public string Visit(PowExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + "^" + expr.Right.Accept(this, scope);
        }

        public string Visit(EqualsExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " == " + expr.Right.Accept(this, scope); 
        }

        public string Visit(NotEqualsExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " != " + expr.Right.Accept(this, scope);
        }

        public string Visit(GreaterThanExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " > " + expr.Right.Accept(this, scope);
        }

        public string Visit(LessThanExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " < " + expr.Right.Accept(this, scope);
        }

        public string Visit(GreaterThanOrEqualsExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " >= " + expr.Right.Accept(this, scope);
        }

        public string Visit(LessThanOrEqualsExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " <= " + expr.Right.Accept(this, scope);
        }


        public string Visit(AndExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " && " + expr.Right.Accept(this, scope);
        }

        public string Visit(OrExpr expr, Scope scope)
        {
            return expr.Left.Accept(this, scope) + " || " + expr.Right.Accept(this, scope);
        }

        public string Visit(NotExpr expr, Scope scope)
        {
            return "!" + expr.Right.Accept(this, scope);
        }

        public string Visit(ConditionalExpr expr, Scope scope)
        {
            return expr.Condition.Accept(this, scope) + " ? " + expr.ThenExpression.Accept(this, scope) + " : " + expr.ElseExpression.Accept(this, scope);
        }

        public string Visit(NegationExpr expr, Scope scope)
        {
            return "-" + expr.Right.Accept(this, scope);
        }
    }
}
