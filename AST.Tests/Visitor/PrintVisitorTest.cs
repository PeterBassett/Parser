using System.Linq;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Visitor;
using NUnit.Framework;

namespace AST.Tests.Visitor
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
            [Test]
            public void AllTakeASingleArgument()
            {
                var visitMethods = from method in typeof (PrintVisitor).GetMethods()
                    where method.Name == "Visit"
                    select method;

                foreach (var method in visitMethods)
                {
                    var parameters = method.GetParameters();
                    Assert.AreEqual(1, parameters.Length);

                    Assert.IsTrue(typeof(IExpression).IsAssignableFrom(parameters.First().ParameterType),
                        "Visit must accept an IExpression instance");
                }
            }

            [Test]
            public void VisitIntegerConstantExpr()
            {
                var target = new PrintVisitor();

                var expression = new ConstantExpr(1234);

                var actual = target.Visit(expression);

                Assert.AreEqual("1234", actual);
            }

            [Test]
            public void VisitFloatConstantExpr()
            {
                var target = new PrintVisitor();

                var expression = new ConstantExpr(12.34);

                var actual = target.Visit(expression);

                Assert.AreEqual("12.34", actual);
            }


            [Test]
            public void VisitPlusExpr()
            {
                var target = new PrintVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression);

                Assert.AreEqual("1+2", actual);
            }

            [Test]
            public void VisitMinusExpr()
            {
                var target = new PrintVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MinusExpr(lhs, rhs);

                var actual = target.Visit(expression);

                Assert.AreEqual("1-2", actual);
            }

            [Test]
            public void VisitMultExpr()
            {
                var target = new PrintVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MultExpr(lhs, rhs);

                var actual = target.Visit(expression);

                Assert.AreEqual("1*2", actual);
            }

            [Test]
            public void VisitDivExpr()
            {
                var target = new PrintVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new DivExpr(lhs, rhs);

                var actual = target.Visit(expression);

                Assert.AreEqual("1/2", actual);
            }

            [Test]
            public void VisitIdentifierExpr()
            {
                var target = new PrintVisitor();

                var expression = new IdentifierExpr("test");

                var actual = target.Visit(expression);

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

                var actual = target.Visit(expression);

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

                var actual = target.Visit(expression);

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

                var actual = target.Visit(expression);

                Assert.AreEqual("e=m*C^2", actual);
            }
        }
    }
}
