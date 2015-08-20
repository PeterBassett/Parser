using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;

namespace AST.Visitor
{
    public interface IExpressionVisitor<T>
    {
        T Visit(IdentifierExpr expr);
        T Visit(PlusExpr expr);
        T Visit(ConstantExpr expr);
        T Visit(DivExpr expr);
        T Visit(MinusExpr expr);
        T Visit(MultExpr expr);
        T Visit(AssignmentExpr expr);
        T Visit(PowExpr expr);
        T Visit(EqualsExpr expr);
        T Visit(NotEqualsExpr expr);
        T Visit(GreaterThanExpr expr);
        T Visit(LessThanExpr expr);
        T Visit(GreaterThanOrEqualsExpr expr);
        T Visit(LessThanOrEqualsExpr expr);
        T Visit(AndExpr expr);
        T Visit(OrExpr expr);
        T Visit(NotExpr expr);
    }
}
