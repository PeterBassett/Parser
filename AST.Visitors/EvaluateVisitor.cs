using System;
using System.CodeDom;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;
using AST.Statements;
using AST.Statements.Loops;

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
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => a + b,
                                    (a, b) => a + b,
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => a + b);
        }

        public Value Visit(ConstantExpr expr, Scope scope)
        {
            return new Value(expr.Value);
        }

        public Value Visit(DivExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => a / b,
                                    (a, b) => a / b,
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => { throw new InvalidOperationException(); });
        }

        public Value Visit(MinusExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => a - b,
                                    (a, b) => a - b,
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => { throw new InvalidOperationException(); });
        }

        public Value Visit(MultExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => a * b,
                                    (a, b) => a * b,
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => { throw new InvalidOperationException(); });
        }

        public Value Visit(AssignmentExpr expr, Scope scope)
        {
            return new Value(expr.Right.Accept(this, scope));
        }

        public Value Visit(PowExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => Math.Pow(a, b),
                                    (a, b) => Math.Pow(a, b),
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => { throw new InvalidOperationException(); });
        }

        public Value Visit(EqualsExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => a.Equals(b),
                                    (a, b) => a == b,
                                    (a, b) => a == b,
                                    (a, b) => a == b);
        }

        public Value Visit(NotEqualsExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => !a.Equals(b),
                                    (a, b) => a != b,
                                    (a, b) => a != b,
                                    (a, b) => a != b);
        }

        public Value Visit(GreaterThanExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => a > b,
                                    (a, b) => a > b,
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => string.CompareOrdinal(a, b) > 0);
        }

        public Value Visit(LessThanExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => a < b,
                                    (a, b) => a < b,
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => string.CompareOrdinal(a, b) < 0);
        }

        public Value Visit(GreaterThanOrEqualsExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => a >= b,
                                    (a, b) => a >= b,
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => string.CompareOrdinal(a, b) >= 0);
        }

        public Value Visit(LessThanOrEqualsExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => a <= b,
                                    (a, b) => a <= b,
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => string.CompareOrdinal(a, b) <= 0);
        }

        public Value Visit(AndExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => a && b,
                                    (a, b) => { throw new InvalidOperationException(); });
        }

        public Value Visit(OrExpr expr, Scope scope)
        {
            return PerformOperation(expr.Left.Accept(this, scope),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => a || b,
                                    (a, b) => { throw new InvalidOperationException(); });
        }

        public Value Visit(NotExpr expr, Scope scope)
        {
            return PerformOperation(new Value(true),
                                    expr.Right.Accept(this, scope),
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => !b,
                                    (a, b) => { throw new InvalidOperationException(); });
        }

        public Value Visit(ConditionalExpr expr, Scope scope)
        {
            var condition = expr.Condition.Accept(this, scope);

            if(condition.GetTypeCode() != TypeCode.Boolean)
                throw new InvalidCastException();

            if (condition.ToBoolean())
                return expr.ThenExpression.Accept(this, scope);

            return expr.ElseExpression.Accept(this, scope);
        }

        public Value Visit(NegationExpr expr, Scope scope)
        {
            return PerformOperation(new Value(0), 
                                    expr.Right.Accept(this, scope),
                                    (a, b) => -b ,
                                    (a, b) => -b ,
                                    (a, b) => { throw new InvalidOperationException(); },
                                    (a, b) => { throw new InvalidOperationException(); });
        }

        static TypeCode PromoteToType(Value lhs, Value rhs)
        {
            var l = lhs.GetTypeCode();
            var r = rhs.GetTypeCode();

            if (lhs.IsNumericType() != rhs.IsNumericType())
                throw new InvalidCastException();

            if (lhs.IsNumericType() && rhs.IsNumericType())
            {
                if (l == TypeCode.Double || r == TypeCode.Double)
                    return TypeCode.Double;

                if (l == TypeCode.Int64 || r == TypeCode.Int64)
                    return TypeCode.Int64;

                throw new InvalidOperationException();
            }

            if (l == TypeCode.String && r == TypeCode.String)
                return TypeCode.String;

            if (l == TypeCode.Boolean && r == TypeCode.Boolean)
                return TypeCode.Boolean;

            throw new InvalidOperationException();
        }

        private static Value PerformOperation(Value lhs, Value rhs,
            Func<double, double, object> DoubleOp,
            Func<long, long, object> IntOp,
            Func<bool, bool, object> BoolOp,
            Func<string, string, object> StringOp)
        {
            var typeCode = PromoteToType(lhs, rhs);

            switch (typeCode)
            {
                case TypeCode.Double:
                    return new Value(DoubleOp(lhs.ToDouble(), rhs.ToDouble()));
                case TypeCode.Int64:
                    return new Value(IntOp(lhs.ToInt(), rhs.ToInt()));
                case TypeCode.Boolean:
                    return new Value(BoolOp(lhs.ToBoolean(), rhs.ToBoolean()));
                case TypeCode.String:
                    return new Value(StringOp(lhs.ToString(), rhs.ToString()));
                default:
                    throw new InvalidCastException();
            }
        }

        public Value Visit(WhileStmt stmt, Scope scope)
        {
            while (stmt.Condition.Accept(this, scope).ToBoolean())
            {
                stmt.Block.Accept(this, scope);
            }

            return new Value();
        }

        public Value Visit(IfStmt stmt, Scope scope)
        {
            var condition = stmt.Condition.Accept(this, scope);

            if (condition.GetTypeCode() != TypeCode.Boolean)
                throw new InvalidCastException();

            if (condition.ToBoolean())
                return stmt.ThenExpression.Accept(this, scope);
            
            return stmt.ElseExpression.Accept(this, scope);
        }

        public Value Visit(BlockStmt stmt, Scope scope)
        {
            foreach (var statement in stmt.Statements)
            {
                statement.Accept(this, scope);
            }

            return new Value();
        }

        public Value Visit(NoOpStatement stmt, Scope scope)
        {
            return new Value();
        }

        public Value Visit(DoWhileStmt stmt, Scope scope)
        {
            do
            {
                stmt.Block.Accept(this, scope);
            } while (stmt.Condition.Accept(this, scope).ToBoolean());

            return new Value();
        }
    }
}
