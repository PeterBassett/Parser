using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Logical;
using AST.Statements;
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

                    Assert.IsTrue(typeof(IExpression).IsAssignableFrom(parameters.First().ParameterType), "Visit must accept an IExpression instance");
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
                                     where typeof(IExpression).IsAssignableFrom(param.ParameterType)
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

                Assert.AreEqual(typeof(int), actual);
            }

            [Test]
            public void VisitFloatConstantExpr()
            {
                var target = new TypeCheckingVisitor();

                var expression = new ConstantExpr(12.34);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(typeof(double), actual);
            }

            [TestCase(true)]
            [TestCase(false)]
            public void VisitBooleanConstantExpr(bool value)
            {
                var target = new TypeCheckingVisitor();

                var expression = new ConstantExpr(value);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(typeof(bool), actual);
            }

            [Test]
            public void VisitPlusExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(typeof(int), actual);
            }

            [Test]
            public void VisitPlusWithDoublesExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1.1);
                var rhs = new ConstantExpr(2.2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(typeof(double), actual);
            }

            [Test]
            public void VisitPlusWithIntAndDoubleExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2.2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(typeof(double), actual);
            }

            [Test]
            public void VisitPlusWithDoubleAndIntExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1.1);
                var rhs = new ConstantExpr(2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(typeof(double), actual);
            }

            [Test]
            public void VisitMinusExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MinusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(typeof(int), actual);
            }

            [Test]
            public void VisitMultExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MultExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(typeof(int), actual);
            }

            [Test]
            public void VisitDivExpr()
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(10);
                var rhs = new ConstantExpr(2);
                var expression = new DivExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(typeof(int), actual);
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

                Assert.AreEqual(typeof(int), actual);
            }

            [TestCase(true, true, typeof(bool))]
            [TestCase(false, true, typeof(bool))]
            [TestCase(true, false, typeof(bool))]
            [TestCase(false, false, typeof(bool))]
            public void VisitBooleanAndExpr(bool a, bool b, Type expected)
            {
                var target = new TypeCheckingVisitor();

                var aExpression = new ConstantExpr(a);
                var bExpression = new ConstantExpr(b);
                var expr = new AndExpr(aExpression, bExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true, true, typeof(bool))]
            [TestCase(false, true, typeof(bool))]
            [TestCase(true, false, typeof(bool))]
            [TestCase(false, false, typeof(bool))]
            public void VisitBooleanOrExpr(bool a, bool b, Type expected)
            {
                var target = new TypeCheckingVisitor();

                var aExpression = new ConstantExpr(a);
                var bExpression = new ConstantExpr(b);
                var expr = new OrExpr(aExpression, bExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true, typeof(bool))]
            [TestCase(false, typeof(bool))]
            public void VisitBooleanNotExpr(bool a, Type expected)
            {
                var target = new TypeCheckingVisitor();

                var aExpression = new ConstantExpr(a);
                var expr = new NotExpr(aExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true, 1, 2, typeof(int))]
            [TestCase(false, 1.0, 2, typeof(double))]
            [TestCase(true, 1234, 5678.0, typeof(double))]
            [TestCase(false, false, false, typeof(bool))]
            public void VisitConditionalExpr(bool condition, object thenValue, object elseValue, Type expected)
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

            [TestCase(5, typeof(int))]
            [TestCase(-5, typeof(int))]
            [TestCase(-5.45, typeof(double))]
            [TestCase(5.45, typeof(double))]
            [TestCase(0, typeof(int))]
            [TestCase(-0, typeof(int))]
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

                Assert.AreEqual(typeof(bool), actual);
            }

            [Test]
            public void AndOrFalseTest()
            {
                var target = new TypeCheckingVisitor();

                var expr = new AndExpr(new OrExpr(new ConstantExpr(true), new ConstantExpr(true)), new ConstantExpr(false));

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(typeof(bool), actual);
            }

            [Test]
            public void AndOrTest()
            {
                var target = new TypeCheckingVisitor();

                var expr = new AndExpr(new ConstantExpr(true), new OrExpr(new ConstantExpr(true), new ConstantExpr(false)));

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(typeof(bool), actual);
            }

            [Test]
            public void AssignmentTest()
            {
                var target = new TypeCheckingVisitor();

                var expr = new AssignmentExpr(new IdentifierExpr("a"), new ConstantExpr(1));

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(typeof(int), actual);
            }

            [TestCase(ExpectedException=typeof(TypeCheckException))]
            public void InvalidLValueAssignmentTest()
            {
                var target = new TypeCheckingVisitor();

                var expr = new AssignmentExpr(new ConstantExpr("a"), new ConstantExpr(1));

                target.Visit(expr, scope);
            }

            [TestCase(1, 2, typeof(PlusExpr), typeof(int))]
            [TestCase(1.0, 2, typeof(PlusExpr), typeof(double))]
            [TestCase(1, 2.0, typeof(PlusExpr), typeof(double))]
            [TestCase("1", "2", typeof(PlusExpr), typeof(string))]
            [TestCase("1.0", 2, typeof(PlusExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase("1.0", 2.0, typeof(PlusExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1, "2", typeof(PlusExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1.0, "2", typeof(PlusExpr), null, ExpectedException = typeof(TypeCheckException))]            
            [TestCase(1, 2, typeof(MinusExpr), typeof(int))]
            [TestCase(1.0, 2, typeof(MinusExpr), typeof(double))]
            [TestCase(1, 2.0, typeof(MinusExpr), typeof(double))]
            [TestCase("1", "2", typeof(MinusExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1, 2, typeof(MultExpr), typeof(int))]
            [TestCase(1.0, 2, typeof(MultExpr), typeof(double))]
            [TestCase(1, 2.0, typeof(MultExpr), typeof(double))]
            [TestCase("1", "2", typeof(MultExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1, 2, typeof(DivExpr), typeof(int))]
            [TestCase(1.0, 2, typeof(DivExpr), typeof(double))]
            [TestCase(1, 2.0, typeof(DivExpr), typeof(double))]
            [TestCase("1", "2", typeof(DivExpr), null, ExpectedException = typeof(TypeCheckException))]
            [TestCase(1, 2, typeof(PowExpr), typeof(int))]
            [TestCase(1.0, 2, typeof(PowExpr), typeof(double))]
            [TestCase(1, 2.0, typeof(PowExpr), typeof(double))]
            [TestCase("1", "2", typeof(PowExpr), null, ExpectedException = typeof(TypeCheckException))]
            public void VisitBinaryOperatorTest(object a, object b, Type expressionType, Type expectedType)
            {
                var target = new TypeCheckingVisitor();

                var lhs = new ConstantExpr(a);
                var rhs = new ConstantExpr(b);

                var expression = (IExpression)Activator.CreateInstance(expressionType, new object[] { lhs, rhs });

                var method =
                    typeof(TypeCheckingVisitor).GetMethods()
                        .First(m => m.Name == "Visit" && m.GetParameters().ElementAt(0).ParameterType == expressionType);

                Type actual = null;

                try
                {
                    actual = (Type)method.Invoke(target, new object[] { expression, scope });
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

                Assert.AreEqual(typeof(void), actual);
            }
        }
    }
}
