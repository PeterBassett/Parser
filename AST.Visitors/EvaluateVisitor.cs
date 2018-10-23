using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EvaluateVisitor : IExpressionVisitor<Value, Scope>
    {
        public Value Visit(IdentifierExpr expr, Scope scope)
        {
            var value = scope.FindIdentifier(expr.Name);
            return value.Value;
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
            return Value.FromObject(expr.Value);
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
            var name = expr.LValueExpression.Name;
            var variable = scope.FindIdentifier(name);

            if(!variable.IsDefined)
                throw new UndefinedIdentifierException("Identifier is not defined" + expr.LValueExpression.Name);
                
            var rvalue = expr.Right.Accept(this, scope);

            // HACK : Need to rationalise the type checking.
            // It's either in here or it's not!
            //if(variable.Value.Type != rvalue.Type)
             //   throw new InvalidCastException();

            scope.AssignIdentifierValue(name, rvalue);

            return rvalue;
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

            if(condition.Type != ValueType.Boolean)
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

        static ValueType PromoteToType(Value lhs, Value rhs)
        {
            var l = lhs.Type;
            var r = rhs.Type;

            if (lhs.IsNumericType() != rhs.IsNumericType())
                throw new InvalidCastException();

            if (lhs.IsNumericType() && rhs.IsNumericType())
            {
                if (l == ValueType.Float || r == ValueType.Float)
                    return ValueType.Float;

                if (l == ValueType.Int || r == ValueType.Int)
                    return ValueType.Int;

                throw new InvalidOperationException();
            }

            if (l == ValueType.String && r == ValueType.String)
                return ValueType.String;

            if (l == ValueType.Boolean && r == ValueType.Boolean)
                return ValueType.Boolean;

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
                case ValueType.Float:
                    return Value.FromObject(DoubleOp(lhs.ToDouble(), rhs.ToDouble()));
                case ValueType.Int:
                    return Value.FromObject(IntOp(lhs.ToInt(), rhs.ToInt()));
                case ValueType.Boolean:
                    return Value.FromObject(BoolOp(lhs.ToBoolean(), rhs.ToBoolean()));
                case ValueType.String:
                    return Value.FromObject(StringOp(lhs.ToString(), rhs.ToString()));
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

            return Value.Unit;
        }

        public Value Visit(IfStmt stmt, Scope scope)
        {
            var condition = stmt.Condition.Accept(this, scope);

            if (condition.Type != ValueType.Boolean)
                throw new InvalidCastException();

            if (condition.ToBoolean())
                return stmt.ThenExpression.Accept(this, scope);
            
            return stmt.ElseExpression.Accept(this, scope);
        }

        public Value Visit(ScopeBlockStmt stmt, Scope scope)
        {
            using (scope.PushScope())
            {
                foreach (var statement in stmt.Statements)
                {
                    statement.Accept(this, scope);
                }
            }

            return Value.Unit;
        }

        public Value Visit(NoOpStatement stmt, Scope scope)
        {
            return Value.Unit;;
        }

        public Value Visit(DoWhileStmt stmt, Scope scope)
        {
            do
            {
                stmt.Block.Accept(this, scope);
            } while (stmt.Condition.Accept(this, scope).ToBoolean());

            return Value.Unit;;
        }

        public Value Visit(FunctionDefinitionExpr expr, Scope scope)
        {
            var value = Value.FromObject(expr);
            scope.DefineIdentifier(expr.Name, value, value.Type.ToString());
            return new Value( expr );
        }

        public Value Visit(ReturnStmt expr, Scope scope)
        {
            throw new ReturnStatementException(expr.ReturnExpression.Accept(this, scope));
        }

        public Value Visit(VarDefinitionStmt stmt, Scope scope)
        {
            Value value = null;

            if(stmt.InitialValue != null)
                value = stmt.InitialValue.Accept(this, scope);

            scope.DefineIdentifier(stmt.Name.Name, value, stmt.Type.Name);
            return value;
        }

        public Value Visit(FunctionCallExpr expr, Scope scope)
        {
            var function = scope.FindIdentifier(expr.FunctionName.Name);
            
            if(!function.IsDefined)
                throw new UndefinedIdentifierException("Undefined function " + expr.FunctionName.Name);

            if(!function.Value.IsFunction)
                throw new UndefinedIdentifierException("Identifier is not a function " + expr.FunctionName.Name);            

            var arguments = from argument in expr.Arguments
                            select argument.Accept(this, scope);

            return ExecuteFunction(scope, function.Value.ToFuntion(), arguments);
        }

        private Value ExecuteFunction(Scope scope, FunctionExpr func, IEnumerable<Value> arguments)
        {
            using (scope.PushArguments(func.Arguments, arguments.ToArray()))
            {
                return RunFunction((dynamic)func, scope);
            }
        }

        private Value RunFunction(FunctionDefinitionExpr func, Scope scope)
        {
            try
            {
                func.Body.Accept(this, scope);

                throw new ReturnStatementExpectedException("Function without a return statement encountered "  + func.Name);
            }
            catch (ReturnStatementException returnStatement)
            {
                return returnStatement.Value;
            }
        }

        private Value RunFunction(LambdaDefinitionExpr func, Scope scope)
        {
            return func.Body.Accept(this, scope);            
        }

        public Value Visit(LambdaDefinitionExpr lambda, Scope scope)
        {
            var value = Value.FromObject(lambda);
            scope.DefineIdentifier(lambda.Name, value, value.Type.ToString());
            return value;
        }


        public Value Visit(StatementList stmt, Scope scope)
        {
            // no new scope for a simple statement list.
            foreach (var statement in stmt.Statements)
            {
                statement.Accept(this, scope);
            }
            
            return Value.Unit;
        }


        public Value Visit(ClassDefinitionStmt classDefinitionStmt, Scope context)
        {
            return Value.Unit;
        }
    }
}
