using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;
using NUnit.Framework;

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
            private static Scope scope  = new Scope();

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

                    Assert.IsTrue(typeof(IExpression).IsAssignableFrom(parameters.First().ParameterType), "Visit must accept an IExpression instance");
                    Assert.IsTrue(typeof(Scope).IsAssignableFrom(parameters.ElementAt(1).ParameterType), "Visit must accept an Scope instance");
                }
            }

            [Test]
            public void NoneShouldThrowNotImplementedException()
            {
                var visitMethods = from method in typeof(EvaluateVisitor).GetMethods()
                                   where method.Name == "Visit"
                                   select method;

                var target = new PrintVisitor();

                var errors = new List<string>();
                foreach (var method in visitMethods)
                {
                    var parameter = (from param in method.GetParameters()
                                     where typeof(IExpression).IsAssignableFrom(param.ParameterType)
                                     select param).First();

                    try
                    {
                        method.Invoke(target, new object[] { null });
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

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("1234", actual.ToString());
            }

            [Test]
            public void VisitFloatConstantExpr()
            {
                var target = new EvaluateVisitor();

                var expression = new ConstantExpr(12.34);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("12.34", actual.ToString());
            }

            [TestCase(true)]
            [TestCase(false)]
            public void VisitBooleanConstantExpr(bool value)
            {
                var target = new EvaluateVisitor();

                var expression = new ConstantExpr(value);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual(value.ToString(), actual.ToString());
            }

            [Test]
            public void VisitPlusExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("3", actual.ToString());
            }

            [Test]
            public void VisitMinusExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MinusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("-1", actual.ToString());
            }

            [Test]
            public void VisitMultExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MultExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("2", actual.ToString());
            }

            [Test]
            public void VisitDivExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(10);
                var rhs = new ConstantExpr(2);
                var expression = new DivExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

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

                var actual = target.Visit(expr, scope);

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

                var actual = target.Visit(expr, scope);

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

                var actual = target.Visit(expr, scope);

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

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual.ToBoolean());
            }

            [TestCase(true, false)]
            [TestCase(false, true)]
            public void VisitBooleanNotExpr(bool a, bool expected)
            {
                var target = new EvaluateVisitor();

                var aExpression = new ConstantExpr(a);
                var expr = new NotExpr(aExpression);

                var actual = target.Visit(expr, scope);

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

                var actual = target.Visit(expr, scope);

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

                target.Visit(expr, scope);
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

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual.ToObject());
            }

            [Test]
            public void OrAndTest()
            {
                var target = new EvaluateVisitor();

                var expr = new OrExpr(new ConstantExpr(true), new AndExpr(new ConstantExpr(true), new ConstantExpr(false)));

                var actual = target.Visit(expr, scope); ;

                Assert.AreEqual(true, actual.ToBoolean());
            }

            [Test]
            public void AndOrFalseTest()
            {
                var target = new EvaluateVisitor();

                var expr = new AndExpr(new OrExpr(new ConstantExpr(true), new ConstantExpr(true)), new ConstantExpr(false));

                var actual = target.Visit(expr, scope); ;

                Assert.AreEqual(false, actual.ToBoolean());
            }

            [Test]
            public void AndOrTest()
            {
                var target = new EvaluateVisitor();

                var expr = new AndExpr(new ConstantExpr(true), new OrExpr(new ConstantExpr(true), new ConstantExpr(false)));

                var actual = target.Visit(expr, scope); ;

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

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual.ToBoolean());
            }
        }
    }
}
