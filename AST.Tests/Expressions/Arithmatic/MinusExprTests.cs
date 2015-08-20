using System;
using AST.Expressions.Arithmatic;
using AST.Visitor;
using Moq;
using NUnit.Framework;

namespace AST.Tests.Expressions.Arithmatic
{
    namespace MinusExprTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase(false, false, ExpectedException = typeof (ArgumentNullException))]
            [TestCase(true, false, ExpectedException = typeof (ArgumentNullException))]
            [TestCase(false, true, ExpectedException = typeof (ArgumentNullException))]
            [TestCase(true, true)]
            public void ThrowsOnNulls(bool lhsSupplied, bool rhsSupplied)
            {
                IExpression lhs = lhsSupplied ? new ConstantExpr(1) : null;
                IExpression rhs = rhsSupplied ? new ConstantExpr(1) : null;

                new MinusExpr(lhs, rhs);
            }
        }

        [TestFixture]
        public class Properties
        {
            [Test]
            public void LeftPropertyReturnsPassedLhsInstance()
            {
                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);

                var target = new MinusExpr(lhs, rhs);

                Assert.AreSame(lhs, target.Left);
            }

            [Test]
            public void RightPropertyReturnsPassedRhsInstance()
            {
                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);

                var target = new MinusExpr(lhs, rhs);

                Assert.AreSame(rhs, target.Right);
            }

            [Test]
            public void OperatorPropertyReturnMinusSymbol()
            {
                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);

                var target = new MinusExpr(lhs, rhs);

                Assert.AreEqual("-", target.Operator);
            }
        }

        [TestFixture]
        public class ConstantExprVisitorTests
        {
            [Test]
            public void AcceptMethodCallsVisitOnVisitorWithThis()
            {
                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);

                var target = new MinusExpr(lhs, rhs);
                var visitor = new Mock<IExpressionVisitor<string>>();

                target.Accept(visitor.Object);

                visitor.Verify(x => x.Visit(target), Times.Once);
            }

            [Test]
            public void AcceptMethodCallsOnlyVisitOnVisitorWithThisAndNoOtherVisitMethods()
            {
                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);

                var target = new MinusExpr(lhs, rhs);
                // throw exception is any other methods called other than the MinusExpr overload.
                var visitor = new Mock<IExpressionVisitor<string>>(MockBehavior.Strict);
                visitor.Setup(x => x.Visit(It.IsAny<MinusExpr>())).Returns("");

                target.Accept(visitor.Object);
            }
        }
    }
}
