using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Logical;
using NUnit.Framework;

namespace AST.Visitor.Tests
{
    namespace PrintVisitorTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase]
            public void TakesNoArguments()
            {
                new PrintVisitor();
            }
        }

        [TestFixture]
        public class VisitMethods
        {
            private static Scope scope = new Scope();

            [Test]
            public void AllTakeTwoArgumentsArgument()
            {
                var visitMethods = from method in typeof (PrintVisitor).GetMethods()
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
                var visitMethods = from method in typeof(PrintVisitor).GetMethods()
                                   where method.Name == "Visit"
                                   select method;

                var target = new PrintVisitor();

                var errors = new List<string>();
                foreach (var method in visitMethods)
                {
                    var parameter = (from param in method.GetParameters()
                        where typeof (IExpression).IsAssignableFrom(param.ParameterType)
                        select param).First();

                    try
                    {
                        method.Invoke(target, new object[] {null, null});
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
                var target = new PrintVisitor();

                var expression = new ConstantExpr(1234);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("1234", actual);
            }

            [Test]
            public void VisitFloatConstantExpr()
            {
                var target = new PrintVisitor();

                var expression = new ConstantExpr(12.34);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("12.34", actual);
            }


            [Test]
            public void VisitPlusExpr()
            {
                var target = new PrintVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("1+2", actual);
            }

            [Test]
            public void VisitMinusExpr()
            {
                var target = new PrintVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MinusExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("1-2", actual);
            }

            [Test]
            public void VisitMultExpr()
            {
                var target = new PrintVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MultExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("1*2", actual);
            }

            [Test]
            public void VisitDivExpr()
            {
                var target = new PrintVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new DivExpr(lhs, rhs);

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("1/2", actual);
            }

            [Test]
            public void VisitIdentifierExpr()
            {
                var target = new PrintVisitor();

                var expression = new IdentifierExpr("test");

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("test", actual);
            }

            [Test]
            public void VisitMathExpressionTree()
            {
                var target = new PrintVisitor();

                var one = new ConstantExpr(1);
                var two = new ConstantExpr(2);
                var three = new ConstantExpr(3);
                var four = new ConstantExpr(4);
                var five = new ConstantExpr(5);
                var six = new ConstantExpr(6);

                var expression = new DivExpr(new MultExpr(three, six), new MultExpr(new MinusExpr(five, one), new PlusExpr(four, two)));

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("3*6/5-1*4+2", actual);
            }

            [Test]
            public void VisitAlgebraicExpressionTree()
            {
                var target = new PrintVisitor();

                var a = new IdentifierExpr("a");
                var b = new IdentifierExpr("b");
                var c = new IdentifierExpr("c");
                var one = new ConstantExpr(1);
                var two = new ConstantExpr(2);
                var three = new ConstantExpr(3);
                var four = new ConstantExpr(4);
                var five = new ConstantExpr(5);
                var six = new ConstantExpr(6);

                var expression = new DivExpr(new MultExpr(three,new MultExpr(a, two)), new MultExpr(new MinusExpr(new PowExpr(five,b), one), new PlusExpr(new MinusExpr(six, four), c)));

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("3*a*2/5^b-1*6-4+c", actual);
            }

            [Test]
            public void VisitMassEnergyEquavalenceExpressionTree()
            {
                var target = new PrintVisitor();

                var e = new IdentifierExpr("e");
                var m = new IdentifierExpr("m");
                var c = new IdentifierExpr("C");
                var two = new ConstantExpr(2);

                var expression = new AssignmentExpr(e, new MultExpr(m, new PowExpr(c, two)));

                var actual = target.Visit(expression, scope);

                Assert.AreEqual("e=m*C^2", actual);
            }

            [TestCase(true, true, "True && True")]
            [TestCase(false, true, "False && True")]
            [TestCase(1, 2, "1 && 2")]
            public void VisitBooleanAndExpr(object a, object b, string expected)
            {
                var target = new PrintVisitor();

                var aExpression = new ConstantExpr(a);
                var bExpression = new ConstantExpr(b);
                var expr = new AndExpr(aExpression, bExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true, true, "True || True")]
            [TestCase(false, true, "False || True")]
            [TestCase(1, 2, "1 || 2")]
            public void VisitBooleanOrExpr(object a, object b, string expected)
            {
                var target = new PrintVisitor();

                var aExpression = new ConstantExpr(a);
                var bExpression = new ConstantExpr(b);
                var expr = new OrExpr(aExpression, bExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true, "!True")]
            [TestCase(false, "!False")]
            public void VisitBooleanNotExpr(bool a, string expected)
            {
                var target = new PrintVisitor();

                var aExpression = new ConstantExpr(a);
                var expr = new NotExpr(aExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }

            [TestCase(true, 1, 2, "True ? 1 : 2")]
            [TestCase(false, 1, 2, "False ? 1 : 2")]
            [TestCase(true, 1234, 5678, "True ? 1234 : 5678")]
            [TestCase(false, 1234, 5678, "False ? 1234 : 5678")]
            public void VisitConditionalExpr(bool condition, int thenValue, int elseValue, string expected)
            {
                var target = new PrintVisitor();

                var conditionExpression = new ConstantExpr(condition);
                var thenExpression = new ConstantExpr(thenValue);
                var elseExpression = new ConstantExpr(elseValue);

                var expr = new ConditionalExpr(conditionExpression, thenExpression, elseExpression);

                var actual = target.Visit(expr, scope);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
