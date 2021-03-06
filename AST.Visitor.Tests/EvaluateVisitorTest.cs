﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;
using AST.Statements;
using AST.Statements.Loops;
using AST.Visitor.Exceptions;
using Moq;
using NUnit.Framework;
using AST.Expressions.Function;
using TestHelpers;

namespace AST.Visitor.Tests
{
    namespace EvaluateVisitorTestTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase]
            public void TakesNoArguments()
            {
                new EvaluateVisitor();
            }
        }

        [TestFixture]
        public class VisitMethods
        {
            private Scope _scope;

            [SetUp]
            public void DefineScope()
            {
                _scope = new Scope();
            }

            [Test]
            public void AllTakeTwoArgumentsArgument()
            {
                var visitMethods = from method in typeof (EvaluateVisitor).GetMethods()
                    where method.Name == "Visit"
                    select method;

                foreach (var method in visitMethods)
                {
                    var parameters = method.GetParameters();
                    Assert.AreEqual(2, parameters.Length);

                    Assert.IsTrue(typeof(Expression).IsAssignableFrom(parameters.First().ParameterType), "Visit must accept an Expression instance");
                    Assert.IsTrue(typeof(Scope).IsAssignableFrom(parameters.ElementAt(1).ParameterType), "Visit must accept an Scope instance");
                }
            }

            [Test]
            public void NoneShouldThrowNotImplementedException()
            {
                var visitMethods = from method in typeof(EvaluateVisitor).GetMethods()
                                   where method.Name == "Visit"
                                   select method;

                var target = new EvaluateVisitor();

                var errors = new List<string>();
                foreach (var method in visitMethods)
                {
                    var parameter = (from param in method.GetParameters()
                                     where typeof(Expression).IsAssignableFrom(param.ParameterType)
                                     select param).First();

                    try
                    {
                        method.Invoke(target, new object[] { null, null });
                    }
                    catch (TargetParameterCountException)
                    {
                        Assert.Fail("Parameter Count Error in Reflection Call.");
                    }
                    catch (Exception e)
                    {
                        if (e is NotImplementedException || e.InnerException is NotImplementedException)
                        {
                            // Not implemented! This is unacceptable!
                            errors.Add(string.Format("Unimplemented Visit method for type {0} found.",
                                parameter.ParameterType.FullName));
                        }

                        // all other exception types are fine as we are passing null to the methods. We would expect them to throw.
                    }
                }

                if (errors.Any())
                {
                    string message = string.Join("\r\n", errors.ToArray());
                    Assert.Fail(message);
                }
            }

            [Test]
            public void VisitIntegerConstantExpr()
            {
                var target = new EvaluateVisitor();

                var expression = new ConstantExpr(1234);

                var actual = target.Visit(expression, _scope);

                Assert.AreEqual("1234", actual.ToString());
            }

            [Test]
            public void VisitFloatConstantExpr()
            {
                var target = new EvaluateVisitor();

                var expression = new ConstantExpr(12.34);

                var actual = target.Visit(expression, _scope);

                Assert.AreEqual("12.34", actual.ToString());
            }

            [TestCase(true)]
            [TestCase(false)]
            public void VisitBooleanConstantExpr(bool value)
            {
                var target = new EvaluateVisitor();

                var expression = new ConstantExpr(value);

                var actual = target.Visit(expression, _scope);

                Assert.AreEqual(value.ToString(), actual.ToString());
            }

            [Test]
            public void VisitPlusExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, _scope);

                Assert.AreEqual("3", actual.ToString());
            }

            [Test]
            public void VisitMinusExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MinusExpr(lhs, rhs);

                var actual = target.Visit(expression, _scope);

                Assert.AreEqual("-1", actual.ToString());
            }

            [Test]
            public void VisitMultExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MultExpr(lhs, rhs);

                var actual = target.Visit(expression, _scope);

                Assert.AreEqual("2", actual.ToString());
            }

            [Test]
            public void VisitDivExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(10);
                var rhs = new ConstantExpr(2);
                var expression = new DivExpr(lhs, rhs);

                var actual = target.Visit(expression, _scope);

                Assert.AreEqual("5", actual.ToString());
            }

            [Test]
            public void VisitMathExpressionUsingIntegerDivisionTree()
            {
                var target = new EvaluateVisitor();

                var one = new ConstantExpr(1);
                var two = new ConstantExpr(2);
                var three = new ConstantExpr(3);
                var four = new ConstantExpr(4);
                var five = new ConstantExpr(5);
                var six = new ConstantExpr(6);

                var expr = new DivExpr(new MultExpr(three, six), new MultExpr(new MinusExpr(five, one), new PlusExpr(four, two)));

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual("0", actual.ToString());
            }

            [Test]
            public void VisitMathExpressionUsingFloatingPointDivisionTree()
            {
                var target = new EvaluateVisitor();

                var one = new ConstantExpr(1.0);
                var two = new ConstantExpr(2);
                var three = new ConstantExpr(3);
                var four = new ConstantExpr(4);
                var five = new ConstantExpr(5);
                var six = new ConstantExpr(6);

                var expr = new DivExpr(new MultExpr(three, six), new MultExpr(new MinusExpr(five, one), new PlusExpr(four, two)));

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual("0.75", actual.ToString());
            }

            [TestCase(true, true, true)]
            [TestCase(false, true, false)]
            [TestCase(true, false, false)]
            [TestCase(false, false, false)]
            public void VisitBooleanAndExpr(bool a, bool b, bool expected)
            {
                var target = new EvaluateVisitor();

                var aExpression = new ConstantExpr(a);
                var bExpression = new ConstantExpr(b);
                var expr = new AndExpr(aExpression, bExpression);

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(expected, actual.ToBoolean());
            }

            [TestCase(true, true, true)]
            [TestCase(false, true, true)]
            [TestCase(true, false, true)]
            [TestCase(false, false, false)]
            public void VisitBooleanOrExpr(bool a, bool b, bool expected)
            {
                var target = new EvaluateVisitor();

                var aExpression = new ConstantExpr(a);
                var bExpression = new ConstantExpr(b);
                var expr = new OrExpr(aExpression, bExpression);

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(expected, actual.ToBoolean());
            }

            [TestCase(true, false)]
            [TestCase(false, true)]
            public void VisitBooleanNotExpr(bool a, bool expected)
            {
                var target = new EvaluateVisitor();

                var aExpression = new ConstantExpr(a);
                var expr = new NotExpr(aExpression);

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(expected, actual.ToBoolean());
            }

            [TestCase(true, 1, 2, 1)]
            [TestCase(false, 1, 2, 2)]
            [TestCase(true, 1234, 5678, 1234)]
            [TestCase(false, 1234, 5678, 5678)]
            public void VisitConditionalExpr(bool condition, int thenValue, int elseValue, int expected)
            {
                var target = new EvaluateVisitor();

                var conditionExpression = new ConstantExpr(condition);
                var thenExpression = new ConstantExpr(thenValue);
                var elseExpression = new ConstantExpr(elseValue);

                var expr = new ConditionalExpr(conditionExpression, thenExpression, elseExpression);

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(expected, actual.ToObject());
            }

            [TestCase(true)]
            [TestCase(false)]
            [TestCase(1, ExpectedException = typeof(InvalidCastException))]
            [TestCase(0, ExpectedException = typeof(InvalidCastException))]
            [TestCase(1.0, ExpectedException = typeof(InvalidCastException))]
            [TestCase("true", ExpectedException = typeof(InvalidCastException))]
            [TestCase("false", ExpectedException = typeof(InvalidCastException))]
            public void VisitConditionMustBeBooleanType(object condition)
            {
                var target = new EvaluateVisitor();

                var conditionExpression = new ConstantExpr(condition);
                
                var expr = new ConditionalExpr(conditionExpression, new ConstantExpr(0), new ConstantExpr(0));

                target.Visit(expr, _scope);
            }

            [TestCase(5, -5)]
            [TestCase(-5, 5)]
            [TestCase(-5.45, 5.45)]
            [TestCase(5.45, -5.45)]
            [TestCase(0, 0)]
            [TestCase(-0, 0)]
            public void NegationExpressionTest(object value, object expected)
            {
                var target = new EvaluateVisitor();

                var expr = new NegationExpr(new ConstantExpr(value));

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(expected, actual.ToObject());
            }

            [Test]
            public void OrAndTest()
            {
                var target = new EvaluateVisitor();

                var expr = new OrExpr(new ConstantExpr(true), new AndExpr(new ConstantExpr(true), new ConstantExpr(false)));

                var actual = target.Visit(expr, _scope); ;

                Assert.AreEqual(true, actual.ToBoolean());
            }

            [Test]
            public void AndOrFalseTest()
            {
                var target = new EvaluateVisitor();

                var expr = new AndExpr(new OrExpr(new ConstantExpr(true), new ConstantExpr(true)), new ConstantExpr(false));

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(false, actual.ToBoolean());
            }

            [Test]
            public void AndOrTest()
            {
                var target = new EvaluateVisitor();

                var expr = new AndExpr(new ConstantExpr(true), new OrExpr(new ConstantExpr(true), new ConstantExpr(false)));

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(true, actual.ToBoolean());
            }

            [TestCase(1, 1, true)]
            [TestCase(1, 1.0, true)]
            [TestCase(1, 2, false)]
            [TestCase(true, true, true)]
            [TestCase(true, false, false)]
            [TestCase("a", "b", false)]
            [TestCase("a", "a", true)]
            public void EqualsTest(object a, object b, bool expected)
            {
                var target = new EvaluateVisitor();

                var expr = new EqualsExpr( new ConstantExpr(a), new ConstantExpr(b));

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(expected, actual.ToBoolean());
            }

            [Test]
            public void StatementBlockCallsAllStatementsTest()
            {
                var target = new EvaluateVisitor();

                var statement1 = new Mock<Statement>();
                var statement2 = new Mock<Statement>();
                var statement3 = new Mock<Statement>();

                var stmt = new ScopeBlockStmt(new[] { statement1.Object, statement2.Object, statement3.Object });

                target.Visit(stmt, _scope);

                statement1.Verify(s => s.Accept(target, It.IsAny<Scope>()), Times.Once);
                statement2.Verify(s => s.Accept(target, It.IsAny<Scope>()), Times.Once);
                statement3.Verify(s => s.Accept(target, It.IsAny<Scope>()), Times.Once);
            }


            [Test]
            public void StatementBlockAllStatementsAreExecutedInOrderTest()
            {
                var target = new EvaluateVisitor();

                var statement1 = new Mock<Statement>();
                var statement2 = new Mock<Statement>();
                var statement3 = new Mock<Statement>();

                int invocations = 0;
                var invocationOrder = new int[3];

                statement1.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            invocationOrder[0] = invocations;
                            invocations++;
                            return Value.Unit;;
                        });
                statement2.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            invocationOrder[1] = invocations;
                            invocations++;
                            return Value.Unit;;
                        });
                statement3.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            invocationOrder[2] = invocations;
                            invocations++;
                            return Value.Unit;;
                        });

                var stmt = new ScopeBlockStmt(new[] { statement1.Object, statement2.Object, statement3.Object });

                target.Visit(stmt, _scope);

                for (int i = 0; i < invocationOrder.Length; i++)
                {
                    Assert.AreEqual(i, invocationOrder[i]);
                }
            }

            [Test]
            public void WhileLoopTest()
            {
                var target = new EvaluateVisitor();

                const int totalLoopIteration = 3;
                int loopIterations = 0;

                var condition = new Mock<Expression>();
                condition.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            loopIterations++;
                            return new Value(loopIterations <= totalLoopIteration);
                        });

                var statement = new Mock<Statement>();

                var expr = new WhileStmt(condition.Object, new ScopeBlockStmt(new[] { statement.Object }));

                target.Visit(expr, _scope);

                condition.Verify(c => c.Accept(target, _scope), Times.Exactly(totalLoopIteration + 1));
                statement.Verify( s => s.Accept(target, It.IsAny<Scope>()), Times.Exactly(totalLoopIteration) );                
            }

            [Test]
            public void WhileFalseLoopTest()
            {
                var target = new EvaluateVisitor();

                var condition = new Mock<Expression>();
                condition.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            return new Value(false);
                        });

                var statement = new Mock<Statement>();

                var expr = new WhileStmt(condition.Object, new ScopeBlockStmt(new[] { statement.Object }));

                target.Visit(expr, _scope);

                condition.Verify(c => c.Accept(target, _scope), Times.Exactly(1));
                statement.Verify(s => s.Accept(target, _scope), Times.Never());
            }

            [Test]
            public void WhileLoopConditionIgnoredTest()
            {
                var target = new EvaluateVisitor();

                var conditionExecuted = false;
                var condition = new Mock<Expression>();
                condition.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {                            
                            var retVal = new Value(!conditionExecuted);
                            conditionExecuted = true;
                            return retVal;
                        });

                var statement = new Mock<Statement>();
                statement.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            if (!conditionExecuted)
                                throw new Exception("Statement executed before condition evaluated.");
                            return new Value(false);
                        });

                var expr = new WhileStmt(condition.Object, new ScopeBlockStmt(new[] { statement.Object }));

                target.Visit(expr, _scope);

                condition.Verify(c => c.Accept(target, _scope), Times.Exactly(2));
                statement.Verify(s => s.Accept(target, It.IsAny<Scope>()), Times.Once);
            }


            [Test]
            public void DoWhileLoopTest()
            {
                var target = new EvaluateVisitor();

                const int totalLoopIteration = 3;
                int loopIterations = 0;

                var condition = new Mock<Expression>();
                condition.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            loopIterations++;
                            return new Value(loopIterations < totalLoopIteration);
                        });

                var statement = new Mock<Statement>();

                var expr = new DoWhileStmt(condition.Object, new ScopeBlockStmt(new[] { statement.Object }));

                target.Visit(expr, _scope);

                condition.Verify(c => c.Accept(target, _scope), Times.Exactly(totalLoopIteration));
                statement.Verify(s => s.Accept(target, It.IsAny<Scope>()), Times.Exactly(totalLoopIteration));
            }

            [Test]
            public void DoWhileFalseLoopTest()
            {
                var target = new EvaluateVisitor();

                var condition = new Mock<Expression>();
                condition.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            return new Value(false);
                        });

                var statement = new Mock<Statement>();

                var expr = new DoWhileStmt(condition.Object, new ScopeBlockStmt(new[] { statement.Object }));

                target.Visit(expr, _scope);

                condition.Verify(c => c.Accept(target, _scope), Times.Exactly(1));
                statement.Verify(s => s.Accept(target, It.IsAny<Scope>()), Times.Once);
            }

            [Test]
            public void DoWhileLoopConditionIgnoredTest()
            {
                var target = new EvaluateVisitor();

                var conditionExecuted = false;
                var condition = new Mock<Expression>();
                condition.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            conditionExecuted = true;
                            return new Value(false); ;
                        });

                var statement = new Mock<Statement>();
                statement.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>(
                        (v, s) =>
                        {
                            if (conditionExecuted)
                                throw new Exception("Statement executed after condition evaluated.");
                            return new Value(false);
                        });

                var expr = new DoWhileStmt(condition.Object, new ScopeBlockStmt(new[] { statement.Object }));

                target.Visit(expr, _scope);

                condition.Verify(c => c.Accept(target, _scope), Times.Once);
                statement.Verify(s => s.Accept(target, It.IsAny<Scope>()), Times.Once);
            }

            [TestCase(true, true, true)]
            [TestCase(false, true, true, ExpectedException = typeof(ArgumentNullException))]
            [TestCase(true, false, true, ExpectedException = typeof(ArgumentNullException))]
            [TestCase(true, true, false)]            
            public void IfStatementWithNoElseTest(bool conditionSupplied, bool trueStatementSupplied, bool falseStatementSupplied)
            {
                var conditionStmt = conditionSupplied ? new Mock<Expression>().Object : null;
                var trueStmt = trueStatementSupplied ? new Mock<Statement>().Object : null;
                var falseStmt = falseStatementSupplied ? new Mock<Statement>().Object : null;

                new IfStmt(conditionStmt, trueStmt, falseStmt);
            }

            [TestCase(true)]
            [TestCase(false)]
            public void IfStatementWithNoElseTest(bool conditionValue)
            {
                var target = new EvaluateVisitor();

                var condition = new Mock<Expression>();
                condition.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>((v, s) => new Value(conditionValue));

                var trueStmt = new Mock<Statement>();

                var expr = new IfStmt(condition.Object, trueStmt.Object, null);

                target.Visit(expr, _scope);

                condition.Verify(c => c.Accept(target, _scope), Times.Once);
                trueStmt.Verify(s => s.Accept(target, _scope), Times.Exactly(conditionValue ? 1 : 0));
            }

            [TestCase(true)]
            [TestCase(false)]
            public void IfStatementWithElseTest(bool conditionValue)
            {
                var target = new EvaluateVisitor();

                var condition = new Mock<Expression>();
                condition.Setup(c => c.Accept(It.IsAny<IExpressionVisitor<Value, Scope>>(), It.IsAny<Scope>()))
                    .Returns<IExpressionVisitor<Value, Scope>, Scope>((v, s) => new Value(conditionValue));

                var trueStmt = new Mock<Statement>();
                var falseStmt = new Mock<Statement>();

                var expr = new IfStmt(condition.Object, trueStmt.Object, falseStmt.Object);

                target.Visit(expr, _scope);

                condition.Verify(c => c.Accept(target, _scope), Times.Once);
                trueStmt.Verify(s => s.Accept(target, _scope), Times.Exactly(conditionValue ? 1 : 0));
                falseStmt.Verify(s => s.Accept(target, _scope), Times.Exactly(!conditionValue ? 1 : 0));
            }

            [Test]
            public void ReturnStatementVisitTest()
            {
                var target = new EvaluateVisitor();

                var returnValue = RandomGenerator.String();
                var returnStmt = new ReturnStmt(new ConstantExpr(returnValue));

                try
                {
                    target.Visit(returnStmt, _scope);

                    Assert.Fail("No exception thrown");
                }
                catch (ReturnStatementException rse)
                {
                    Assert.AreEqual(returnValue, rse.Value.ToObject());
                }
                catch (Exception)
                {
                    Assert.Fail("Incorrect exception type caught");
                }
            }

            [Test]
            public void FunctionCallTest()
            {
                var target = new EvaluateVisitor();

                var functionName = RandomGenerator.String();
                var functionNameExpr = new IdentifierExpr(functionName);
                var returnValue = RandomGenerator.String();
                var functionDefinition = new FunctionDefinitionExpr(functionNameExpr, new VarDefinitionStmt[0],
                    new ScopeBlockStmt(new[] {new ReturnStmt(new ConstantExpr(returnValue))}), new IdentifierExpr("String"));
                
                var expr = new FunctionCallExpr(functionNameExpr, new Expression[0]);

                _scope.DefineIdentifier(functionName, Value.FromObject(functionDefinition));

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(returnValue, actual.ToObject());
            }

            [Test]
            public void FunctionCallCallsVisitOnAllParametersPassedTest()
            {
                var target = new EvaluateVisitor();

                var functionName = RandomGenerator.String();
                var functionNameExpr = new IdentifierExpr(functionName);                

                var parameters = new[]
                {
                    new VarDefinitionStmt(new IdentifierExpr("A"), new IdentifierExpr("INT"), false, new ConstantExpr(1)),
                    new VarDefinitionStmt(new IdentifierExpr("B"), new IdentifierExpr("STRING"), false, new ConstantExpr("TEST")),
                    new VarDefinitionStmt(new IdentifierExpr("C"), new IdentifierExpr("BOOL"), false, new ConstantExpr(true))
                };

                var values = new Expression[]
                {
                    new ConstantExpr(1),
                    new ConstantExpr(RandomGenerator.String()),
                    new ConstantExpr(true)
                };

                var returnValue = values[1];

                var functionDefinition = new FunctionDefinitionExpr(functionNameExpr, parameters,
                    new ScopeBlockStmt(new[] { new ReturnStmt(returnValue) }), new IdentifierExpr("String"));

                var expr = new FunctionCallExpr(functionNameExpr, values);

                _scope.DefineIdentifier(functionName, Value.FromObject(functionDefinition));

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(((ConstantExpr)returnValue).Value, actual.ToObject());
            }

            [TestCase(ExpectedException = typeof(UndefinedIdentifierException))]
            public void FunctionCallOnUndefinedFuncTest()
            {
                var target = new EvaluateVisitor();

                var functionName = RandomGenerator.String();
                var definedFunctionNameExpr = new IdentifierExpr(functionName);
                var calledFunctionNameExpr = new IdentifierExpr(functionName + "UNDEFINED");
                var returnValue = RandomGenerator.String();

                var functionDefinition = new FunctionDefinitionExpr(definedFunctionNameExpr, new VarDefinitionStmt[0],
                    new ScopeBlockStmt(new[] { new ReturnStmt(new ConstantExpr(returnValue)) }), new IdentifierExpr("String"));

                var expr = new FunctionCallExpr(calledFunctionNameExpr, new Expression[0]);

                _scope.DefineIdentifier(functionName, Value.FromObject(functionDefinition));

                target.Visit(expr, _scope);
            }

            [TestCase(ExpectedException = typeof(UndefinedIdentifierException))]
            public void FunctionCallOnNonFunctionTest()
            {
                var target = new EvaluateVisitor();

                var functionNameExpr = new IdentifierExpr(RandomGenerator.String());

                var expr = new FunctionCallExpr(functionNameExpr, new Expression[0]);

                _scope.DefineIdentifier(functionNameExpr.Name, Value.FromObject(RandomGenerator.String()));

                target.Visit(expr, _scope);
            }

            [TestCase(ExpectedException = typeof(UndefinedIdentifierException))]
            public void FunctionCallWithEmptyScopeTest()
            {
                var target = new EvaluateVisitor();

                var functionName = RandomGenerator.String();
                var calledFunctionNameExpr = new IdentifierExpr(functionName);
                
                var expr = new FunctionCallExpr(calledFunctionNameExpr, new Expression[0]);

                target.Visit(expr, _scope);
            }

            [TestCase(ExpectedException = typeof(ReturnStatementExpectedException))]
            public void FunctionCallWithNoReturnStatementTest()
            {
                var target = new EvaluateVisitor();

                var functionName = RandomGenerator.String();
                var functionNameExpr = new IdentifierExpr(functionName);
                var returnValue = RandomGenerator.String();
                var functionDefinition = new FunctionDefinitionExpr(functionNameExpr, new VarDefinitionStmt[0], new ScopeBlockStmt(new[] { new NoOpStatement()  }), new IdentifierExpr("String"));

                var expr = new FunctionCallExpr(functionNameExpr, new Expression[0]);

                _scope.DefineIdentifier(functionName, Value.FromObject(functionDefinition));

                target.Visit(expr, _scope);
            }

            [Test]
            public void LambdaCallTest()
            {
                var target = new EvaluateVisitor();

                var functionName = RandomGenerator.String();
                var functionNameExpr = new IdentifierExpr(functionName);
                var returnValue = RandomGenerator.String();
                var functionDefinition = new LambdaDefinitionExpr(functionNameExpr, new VarDefinitionStmt[0], new ConstantExpr(returnValue), new IdentifierExpr("String"));

                var expr = new FunctionCallExpr(functionNameExpr, new Expression[0]);

                _scope.DefineIdentifier(functionName, Value.FromObject(functionDefinition));

                var actual = target.Visit(expr, _scope);

                Assert.AreEqual(returnValue, actual.ToObject());
            }
        }
    }
}
