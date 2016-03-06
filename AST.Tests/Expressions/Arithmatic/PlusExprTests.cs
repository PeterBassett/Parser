using System;
using AST.Expressions.Arithmatic;
using AST.Visitor;
using Moq;
using NUnit.Framework;

namespace AST.Tests.Expressions.Arithmatic
{
    namespace PlusExprTests
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
                Expression lhs = lhsSupplied ? new ConstantExpr(1) : null;
                Expression rhs = rhsSupplied ? new ConstantExpr(1) : null;

                new PlusExpr(lhs, rhs);
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

                var target = new PlusExpr(lhs, rhs);

                Assert.AreSame(lhs, target.Left);
            }

            [Test]
            public void RightPropertyReturnsPassedRhsInstance()
            {
                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);

                var target = new PlusExpr(lhs, rhs);

                Assert.AreSame(rhs, target.Right);
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

                var target = new PlusExpr(lhs, rhs);
                var visitor = new Mock<IExpressionVisitor<string, int>>();

                target.Accept(visitor.Object, 0);

                visitor.Verify(x => x.Visit(target, 0), Times.Once);
            }

            [Test]
            public void AcceptMethodCallsOnlyVisitOnVisitorWithThisAndNoOtherVisitMethods()
            {
                var lhs = new ConstantExpr(1);
                var rhs = new ConstantExpr(2);

                var target = new PlusExpr(lhs, rhs);
                // throw exception is any other methods called other than the PlusExpr overload.
                var visitor = new Mock<IExpressionVisitor<string, int>>(MockBehavior.Strict);
                visitor.Setup(x => x.Visit(It.IsAny<PlusExpr>(), It.IsAny<int>())).Returns("");

                target.Accept(visitor.Object, 123);
            }
        }
    }
}
