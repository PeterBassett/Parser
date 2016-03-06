using System;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Function;
using AST.Expressions.Logical;
using AST.Statements;
using AST.Statements.Loops;
using AST.Visitor.Exceptions;

namespace AST.Visitor
{
    public class TypeCheckingVisitor : IExpressionVisitor<ValueType, Scope>
    {        
        public ValueType Visit(IdentifierExpr expr, Scope scope)
        {
            return LookupTypeOfVar(expr, scope);
        }

        private ValueType LookupTypeOfVar(IdentifierExpr expr, Scope scope)
        {
            var identifier = scope.FindIdentifier(expr.Name);

            if(!identifier.IsDefined)
                throw new UndefinedIdentifierException(expr.Name);

            return identifier.Value.Type;
        }

        public ValueType Visit(ConstantExpr expr, Scope scope)
        {
            return Value.GetValueType(expr.Value);
        }

        public ValueType Visit(PlusExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public ValueType Visit(DivExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope, numericTypesOnly: true);
        }

        public ValueType Visit(MinusExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope, numericTypesOnly:true);
        }

        public ValueType Visit(MultExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope, numericTypesOnly: true);
        }

        public ValueType Visit(AssignmentExpr expr, Scope scope)
        {
            if (!(expr.Left is IdentifierExpr))
                throw new TypeCheckException("Assignement target must be an identifier");            

            return expr.Right.Accept(this, scope);
        }

        public ValueType Visit(PowExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope, numericTypesOnly: true);
        }

        public ValueType Visit(EqualsExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public ValueType Visit(NotEqualsExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public ValueType Visit(GreaterThanExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public ValueType Visit(LessThanExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public ValueType Visit(GreaterThanOrEqualsExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public ValueType Visit(LessThanOrEqualsExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public ValueType Visit(AndExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public ValueType Visit(OrExpr expr, Scope scope)
        {
            return BinaryOperatorTypeCheck(expr, scope);
        }

        public ValueType Visit(NotExpr expr, Scope scope)
        {
            var type = expr.Right.Accept(this, scope);
            
            if (type != ValueType.Boolean)
                throw new TypeCheckException("Not operator must have a boolean condition");

            return type;
        }

        private ValueType BinaryOperatorTypeCheck(BinaryOperatorExpr expr, Scope scope, bool numericTypesOnly = false)
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

        private ValueType BinaryOperatorTypeCheck(ValueType lhs, ValueType rhs)
        {
            if (lhs == rhs)
                return lhs;

            return BinaryOpTypeCoersion(lhs, rhs);
        }

        private ValueType BinaryOpTypeCoersion(ValueType lhs, ValueType rhs)
        {
            if (lhs.IsNumericType() != rhs.IsNumericType())
                throw new TypeCheckException("Cannot combine numeric and non numeric types in operators. Type Type casting to the appropriate type.");

            if (lhs == ValueType.Int || lhs == ValueType.Float || rhs == ValueType.Int || rhs == ValueType.Float)
                return ValueType.Float;            

            // types are different.
            return ValueType.Unit;
        }

        public ValueType Visit(ConditionalExpr expr, Scope scope)
        {
            var condition = expr.Condition.Accept(this, scope);

            if (condition != ValueType.Boolean)
                throw new TypeCheckException("Conditional operator must have a boolean condition");

            return BinaryOperatorTypeCheck(expr.ThenExpression.Accept(this, scope), 
                                           expr.ElseExpression.Accept(this, scope));
        }

        public ValueType Visit(NegationExpr expr, Scope scope)
        {
            var type = expr.Right.Accept(this, scope);

            if(!type.IsNumericType())
                throw new TypeCheckException("Negation only defined for numeric types.");

            return type;
        }

        public ValueType Visit(WhileStmt stmt, Scope scope)
        {
            return ValueType.Unit;
        }

        public ValueType Visit(IfStmt stmt, Scope scope)
        {
            return ValueType.Unit;
        }

        public ValueType Visit(ScopeBlockStmt stmt, Scope scope)
        {
            return ValueType.Unit;
        }

        public ValueType Visit(NoOpStatement stmt, Scope scope)
        {
            return ValueType.Unit;
        }

        public ValueType Visit(DoWhileStmt stmt, Scope context)
        {
            return ValueType.Unit;
        }

        public ValueType Visit(FunctionDefinitionExpr expr, Scope context)
        {
            return ValueType.Function;
        }

        public ValueType Visit(ReturnStmt expr, Scope scope)
        {
            return expr.ReturnExpression.Accept(this, scope);
        }

        public ValueType Visit(VarDefinitionStmt stmt, Scope scope)
        {
            ValueType type;

            if (stmt.Type.Name != null)
            {
                if (!ValueType.TryParse(stmt.Type.Name, true, out type))
                    throw new TypeCheckException("Unknown type " + stmt.Type.Name);
            }
            else
            {
                type = stmt.InitialValue.Accept(this, scope);
            }

            return type;
        }

        public ValueType Visit(FunctionCallExpr functionCallExpr, Scope context)
        {
            return functionCallExpr.Accept(this, context);
        }


        public ValueType Visit(LambdaDefinitionExpr lambdaDefinitionExpr, Scope context)
        {
            return ValueType.Function;
        }


        public ValueType Visit(StatementList blockStmt, Scope context)
        {
            return ValueType.Unit;
        }


        public ValueType Visit(ClassDefinitionStmt classDefinitionStmt, Scope context)
        {
            return ValueType.Class;
        }
    }
}
