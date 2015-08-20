
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Visitor;
using Lexer;
using NUnit.Framework;

namespace AST.Tests.Visitor
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
            [Test]
            public void AllTakeASingleArgument()
            {
                var visitMethods = from method in typeof (EvaluateVisitor).GetMethods()
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
                var target = new EvaluateVisitor();

                var expression = new ConstantExpr(1234);

                var actual = target.Visit(expression);

                Assert.AreEqual("1234", actual.ToString());
            }

            [Test]
            public void VisitFloatConstantExpr()
            {
                var target = new EvaluateVisitor();

                var expression = new ConstantExpr(12.34);

                var actual = target.Visit(expression);

                Assert.AreEqual("12.34", actual.ToString());
            }


            [Test]
            public void VisitPlusExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new PlusExpr(lhs, rhs);

                var actual = target.Visit(expression);

                Assert.AreEqual("3", actual.ToString());
            }

            [Test]
            public void VisitMinusExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MinusExpr(lhs, rhs);

                var actual = target.Visit(expression);

                Assert.AreEqual("-1", actual.ToString());
            }

            [Test]
            public void VisitMultExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);
                var expression = new MultExpr(lhs, rhs);

                var actual = target.Visit(expression);

                Assert.AreEqual("2", actual.ToString());
            }

            [Test]
            public void VisitDivExpr()
            {
                var target = new EvaluateVisitor();

                var lhs = new ConstantExpr(10);
                var rhs = new ConstantExpr(2);
                var expression = new DivExpr(lhs, rhs);

                var actual = target.Visit(expression);

                Assert.AreEqual("5", actual.ToString());
            }

            [Test]
            public void VisitMathExpressionTree()
            {
                var target = new EvaluateVisitor();

                var one = new ConstantExpr(1);
                var two = new ConstantExpr(2);
                var three = new ConstantExpr(3);
                var four = new ConstantExpr(4);
                var five = new ConstantExpr(5);
                var six = new ConstantExpr(6);

                var expression = new DivExpr(new MultExpr(three, six), new MultExpr(new MinusExpr(five, one), new PlusExpr(four, two)));

                var actual = target.Visit(expression);

                Assert.AreEqual("0.75", actual.ToString());
            }
        }
    }
}
