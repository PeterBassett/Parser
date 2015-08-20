using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST.Expressions;
using AST.Expressions.Arithmatic;

namespace AST.Visitor
{
    public interface IExpressionVisitor<T>
    {
        T Visit(IdentifierExpr identifierExpr);
        
        T Visit(PlusExpr plusExpr);
        
        T Visit(ConstantExpr constantExpr);
        
        T Visit(DivExpr divExpr);
        
        T Visit(MinusExpr minusExpr);
        
        T Visit(MultExpr multExpr);
        
        T Visit(AssignmentExpr assignmentExpr);
        
        T Visit(PowExpr powExpr);
    }
}
