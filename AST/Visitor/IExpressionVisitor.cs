using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;
using AST.Statements;
using AST.Statements.Loops;

namespace AST.Visitor
{
    public interface IExpressionVisitor<out T, in C>
    {
        T Visit(IdentifierExpr expr, C context);
        T Visit(PlusExpr expr, C context);
        T Visit(ConstantExpr expr, C context);
        T Visit(DivExpr expr, C context);
        T Visit(MinusExpr expr, C context);
        T Visit(MultExpr expr, C context);
        T Visit(AssignmentExpr expr, C context);
        T Visit(PowExpr expr, C context);
        T Visit(EqualsExpr expr, C context);
        T Visit(NotEqualsExpr expr, C context);
        T Visit(GreaterThanExpr expr, C context);
        T Visit(LessThanExpr expr, C context);
        T Visit(GreaterThanOrEqualsExpr expr, C context);
        T Visit(LessThanOrEqualsExpr expr, C context);
        T Visit(AndExpr expr, C context);
        T Visit(OrExpr expr, C context);
        T Visit(NotExpr expr, C context);
        T Visit(ConditionalExpr expr, C context);
        T Visit(NegationExpr expr, C context);
        T Visit(WhileStmt stmt, C context);
        T Visit(IfStmt stmt, C context);
        T Visit(BlockStmt stmt, C context);
        T Visit(NoOpStatement stmt, C context);
    }
}
