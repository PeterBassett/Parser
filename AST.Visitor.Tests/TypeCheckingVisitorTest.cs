using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Logical;
using AST.Statements;
using AST.Visitor.Exceptions;
using NUnit.Framework;

namespace AST.Visitor.Tests
{
    namespace TypeCheckingVisitorTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase]
            public void TakesNoArguments()
            {
                new TypeCheckingVisitor();
            }
        }

        [TestFixture]
        public class VisitMethods
        {
            private static Scope scope = new Scope();

            [Test]
            public void AllTakeTwoArgumentsArgument()
            {
                var visitMethods = from method in typeof(TypeCheckingVisitor).GetMethods()
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
                var visitMethods = from method in typeof(TypeCheckingVisitor).GetMethods()
                                   where method.Name == "Visit"
                                   select method;

                var target = new TypeCheckingVisitor();

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
                    var message = string.Join("\r\n", errors.ToArray());
                    Assert.Fail(message);
                }
            }

            [Test]
            public void VisitIntegerConstantExpr()
            {
                var target = new TypeCheckingVisitor();

                var expression = new ConstantExpr(1234);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Int, actual);
            }

            [Test]
            public void VisitFloatConstantExpr()
            {
                var target = new TypeCheckingVisitor();

                var expression = new ConstantExpr(12.34);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Float, actual);
            }

            [TestCase(true)]
            [TestCase(false)]
            public void VisitBooleanConstantExpr(bool value)
            {
                var target = new TypeCheckingVisitor();

                var expression = new ConstantExpr(value);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Boolean, actual);
            }

            [Test]
            public void VisitPlusExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Int, actual);
            }

            [Test]
            public void VisitPlusWithDoublesExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1.1);
                var rhs = new ConstantExpr(2.2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Float, actual);
            }

            [Test]
            public void VisitPlusWithIntAndDoubleExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2.2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Float, actual);
            }

            [Test]
            public void VisitPlusWithDoubleAndIntExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1.1);
                var rhs = new ConstantExpr(2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Float, actual);
            }

            [Test]
            public void VisitMinusExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MinusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Int, actual);
            }

            [Test]
            public void VisitMultExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MultExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Int, actual);
            }

            [Test]
            public void VisitDivExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(10);
                var rhs = new ConstantExpr(2);
                var expression = new DivExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Int, actual);
            }

            [Test]
            public void VisitMathExpressionTree()
            {
                var target = new TypeCheckingVisitor();

                var one = new ConstantExpr(1);
                var two = new ConstantExpr(2);
                var three = new ConstantExpr(3);
                var four = new ConstantExpr(4);
                var five = new ConstantExpr(5);
                var six = new ConstantExpr(6);

                var expr = new DivExpr(new MultExpr(three, six), new MultExpr(new MinusExpr(five, one), new PlusExpr(four, two)));

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(ValueType.Int, actual);
            }

            [TestCase(true, true, ValueType.Boolean)]
            [TestCase(false, true, ValueType.Boolean)]
            [TestCase(true, false, ValueType.Boolean)]
            [TestCase(false, false, ValueType.Boolean)]
            public void VisitBooleanAndExpr(bool a, bool b, ValueType expected)
            {
                var target = new TypeCheckingVisitor();

                var aExpression = new ConstantExpr(a);
                var bExpression = new ConstantExpr(b);
                var expr = new AndExpr(aExpression, bExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true, true, ValueType.Boolean)]
            [TestCase(false, true, ValueType.Boolean)]
            [TestCase(true, false, ValueType.Boolean)]
            [TestCase(false, false, ValueType.Boolean)]
            public void VisitBooleanOrExpr(bool a, bool b, ValueType expected)
            {
                var target = new TypeCheckingVisitor();

                var aExpression = new ConstantExpr(a);
                var bExpression = new ConstantExpr(b);
                var expr = new OrExpr(aExpression, bExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true, ValueType.Boolean)]
            [TestCase(false, ValueType.Boolean)]
            public void VisitBooleanNotExpr(bool a, ValueType expected)
            {
                var target = new TypeCheckingVisitor();

                var aExpression = new ConstantExpr(a);
                var expr = new NotExpr(aExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true, 1, 2, ValueType.Int)]
            [TestCase(false, 1.0, 2, ValueType.Float)]
            [TestCase(true, 1234, 5678.0, ValueType.Float)]
            [TestCase(false, false, false, ValueType.Boolean)]
            public void VisitConditionalExpr(bool condition, object thenValue, object elseValue, ValueType expected)
            {
                var target = new TypeCheckingVisitor();

                var conditionExpression = new ConstantExpr(condition);
                var thenExpression = new ConstantExpr(thenValue);
                var elseExpression = new ConstantExpr(elseValue);

                var expr = new ConditionalExpr(conditionExpression, thenExpression, elseExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true)]
            [TestCase(false)]
            [TestCase(1, ExpectedException = typeof(TypeCheckException))]
            [TestCase(0, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1.0, ExpectedException = typeof(TypeCheckException))]
            [TestCase("true", ExpectedException = typeof(TypeCheckException))]
            [TestCase("false", ExpectedException = typeof(TypeCheckException))]
            public void VisitConditionMustBeBooleanType(object condition)
            {
                var target = new TypeCheckingVisitor();

                var conditionExpression = new ConstantExpr(condition);

                var expr = new ConditionalExpr(conditionExpression, new ConstantExpr(0), new ConstantExpr(0));

                target.Visit(expr, scope);
            }

            [TestCase(5, ValueType.Int)]
            [TestCase(-5, ValueType.Int)]
            [TestCase(-5.45, ValueType.Float)]
            [TestCase(5.45, ValueType.Float)]
            [TestCase(0, ValueType.Int)]
            [TestCase(-0, ValueType.Int)]
            [TestCase(true, null, ExpectedException = typeof(TypeCheckException))]
            [TestCase("invalid condition", null, ExpectedException = typeof(TypeCheckException))]
            public void NegationExpressionTest(object value, object expected)
            {
                var target = new TypeCheckingVisitor();

                var expr = new NegationExpr(new ConstantExpr(value));

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void OrAndTest()
            {
                var target = new TypeCheckingVisitor();

                var expr = new OrExpr(new ConstantExpr(true), new AndExpr(new ConstantExpr(true), new ConstantExpr(false)));

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(ValueType.Boolean, actual);
            }

            [Test]
            public void AndOrFalseTest()
            {
                var target = new TypeCheckingVisitor();

                var expr = new AndExpr(new OrExpr(new ConstantExpr(true), new ConstantExpr(true)), new ConstantExpr(false));

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(ValueType.Boolean, actual);
            }

            [Test]
            public void AndOrTest()
            {
                var target = new TypeCheckingVisitor();

                var expr = new AndExpr(new ConstantExpr(true), new OrExpr(new ConstantExpr(true), new ConstantExpr(false)));

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(ValueType.Boolean, actual);
            }

            [Test]
            public void AssignmentTest()
            {
                var target = new TypeCheckingVisitor();

                var expr = new AssignmentExpr(new IdentifierExpr("a"), new ConstantExpr(1));

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(ValueType.Int, actual);
            }

            [TestCase(ExpectedException=typeof(TypeCheckException))]
            public void InvalidLValueAssignmentTest()
            {
                var target = new TypeCheckingVisitor();

                var expr = new AssignmentExpr(new ConstantExpr("a"), new ConstantExpr(1));

                target.Visit(expr, scope);
            }

            [TestCase(1, 2, typeof(PlusExpr), ValueType.Int)]
            [TestCase(1.0, 2, typeof(PlusExpr), ValueType.Float)]
            [TestCase(1, 2.0, typeof(PlusExpr), ValueType.Float)]
            [TestCase("1", "2", typeof(PlusExpr), ValueType.String)]
            [TestCase("1.0", 2, typeof(PlusExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase("1.0", 2.0, typeof(PlusExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1, "2", typeof(PlusExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1.0, "2", typeof(PlusExpr), null, ExpectedException = typeof(TypeCheckException))]            
            [TestCase(1, 2, typeof(MinusExpr), ValueType.Int)]
            [TestCase(1.0, 2, typeof(MinusExpr), ValueType.Float)]
            [TestCase(1, 2.0, typeof(MinusExpr), ValueType.Float)]
            [TestCase("1", "2", typeof(MinusExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1, 2, typeof(MultExpr), ValueType.Int)]
            [TestCase(1.0, 2, typeof(MultExpr), ValueType.Float)]
            [TestCase(1, 2.0, typeof(MultExpr), ValueType.Float)]
            [TestCase("1", "2", typeof(MultExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1, 2, typeof(DivExpr), ValueType.Int)]
            [TestCase(1.0, 2, typeof(DivExpr), ValueType.Float)]
            [TestCase(1, 2.0, typeof(DivExpr), ValueType.Float)]
            [TestCase("1", "2", typeof(DivExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1, 2, typeof(PowExpr), ValueType.Int)]
            [TestCase(1.0, 2, typeof(PowExpr), ValueType.Float)]
            [TestCase(1, 2.0, typeof(PowExpr), ValueType.Float)]
            [TestCase("1", "2", typeof(PowExpr), null, ExpectedException = typeof(TypeCheckException))]
            public void VisitBinaryOperatorTest(object a, object b, Type expressionType, ValueType expectedType)
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(a);
                var rhs = new ConstantExpr(b);

                var expression = (Expression)Activator.CreateInstance(expressionType, new object[] { lhs, rhs });

                var method =
                    typeof(TypeCheckingVisitor).GetMethods()
                        .First(m => m.Name == "Visit" && m.GetParameters().ElementAt(0).ParameterType == expressionType);

                ValueType actual;

                try
                {
                    actual = (ValueType)method.Invoke(target, new object[] { expression, scope });
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }

                Assert.AreEqual(expectedType, actual);
            }

            [Test]
            public void VisitNoOpExpr()
            {
                var target = new TypeCheckingVisitor();

                var expression = new NoOpStatement();

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(ValueType.Unit, actual);
            }
        }
    }
}
