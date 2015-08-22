using System;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;

namespace AST.Visitor
{
    public class TypeCheckingVisitor : IExpressionVisitor<Type, Scope>
    {        
        public Type Visit(IdentifierExpr expr, Scope scope)
        {
            return LookupTypeOfVar(expr, scope);
        }

        private Type LookupTypeOfVar(IdentifierExpr expr, Scope scope)
        {
            var identifier = scope.FindIdentifier(expr.Name);

            if(!identifier.IsDefined)
                throw new UndefinedIdentifierException(expr.Name);

            return identifier.GetCurrentValue().GetType();
        }

        public Type Visit(ConstantExpr expr, Scope scope)
        {
            return expr.Value.GetType();
        }

        public Type Visit(PlusExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public Type Visit(DivExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope, numericTypesOnly: true);
        }

        public Type Visit(MinusExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope, numericTypesOnly:true);
        }

        public Type Visit(MultExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope, numericTypesOnly: true);
        }

        public Type Visit(AssignmentExpr expr, Scope scope)
        {
            if (!(expr.Left is IdentifierExpr))
                throw new TypeCheckException("Assignement target must be an identifier");            

            return expr.Right.Accept(this, scope);
        }

        public Type Visit(PowExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope, numericTypesOnly: true);
        }

        public Type Visit(EqualsExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public Type Visit(NotEqualsExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public Type Visit(GreaterThanExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public Type Visit(LessThanExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public Type Visit(GreaterThanOrEqualsExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public Type Visit(LessThanOrEqualsExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public Type Visit(AndExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public Type Visit(OrExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public Type Visit(NotExpr expr, Scope scope)
        {
            var type = expr.Right.Accept(this, scope);
            
            if (type != typeof(bool))
                throw new TypeCheckException("Not operator must have a boolean condition");

            return type;
        }

        private Type BinaryOperatorTypeCheck(BinaryOperatorExpr expr, Scope scope, bool numericTypesOnly = false)
        {
            var lhs = expr.Left.Accept(this, scope);
            var rhs = expr.Right.Accept(this, scope);

            if (numericTypesOnly)
            {
                if(!lhs.IsNumericType())
                    throw new TypeCheckException("Numeric Types Only");

                if (!rhs.IsNumericType())
                    throw new TypeCheckException("Numeric Types Only");
            }

            return BinaryOperatorTypeCheck(lhs, rhs);
        }

        private Type BinaryOperatorTypeCheck(Type lhs, Type rhs)
        {
            if (lhs == rhs)
                return lhs;

            return BinaryOpTypeCoersion(lhs, rhs);
        }

        private Type BinaryOpTypeCoersion(Type l, Type r)
        {
            var lhs = Type.GetTypeCode(l);
            var rhs = Type.GetTypeCode(r);

            if (l.IsNumericType() != r.IsNumericType())
                throw new TypeCheckException("Cannot combine numeric and non numeric types in operators. Type Type casting to the appropriate type.");

            if (lhs == TypeCode.Int32 || lhs == TypeCode.Double || rhs == TypeCode.Int32 || rhs == TypeCode.Double)
                return typeof(double);            

            // types are different.
            return typeof (object);
        }

        public Type Visit(ConditionalExpr expr, Scope scope)
        {
            var condition = expr.Condition.Accept(this, scope);

            if (condition != typeof (bool))
                throw new TypeCheckException("Conditional operator must have a boolean condition");

            return BinaryOperatorTypeCheck(expr.ThenExpression.Accept(this, scope), 
                                           expr.ElseExpression.Accept(this, scope));
        }

        public Type Visit(NegationExpr expr, Scope scope)
        {
            var type = expr.Right.Accept(this, scope);

            if(!type.IsNumericType())
                throw new TypeCheckException("Negation only defined for numeric types.");

            return type;
        }
    }
}
