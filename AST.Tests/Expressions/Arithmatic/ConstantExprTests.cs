using AST.Expressions.Arithmatic;
using AST.Visitor;
using Moq;
using NUnit.Framework;

namespace AST.Tests.Expressions.Arithmatic
{
    [TestFixture]
    public class ConstantExprTests
    {
        [TestCase(1)]
        [TestCase(1.2)]
        [TestCase("test")]
        [TestCase(true)]
        public void ConstructorTakesAnObject(object value)
        {
            new ConstantExpr(value);
        }

        [TestCase(1)]
        [TestCase(1.2)]
        [TestCase("test")]
        [TestCase(true)]
        public void ValueStoredIsAvailableFromTheValueProperty(object value)
        {
            var target = new ConstantExpr(value);

            Assert.AreSame(value, target.Value);
        }
    }

    [TestFixture]
    public class ConstantExprVisitorTests
    {
        [Test]
        public void AcceptMethodCallsVisitOnVisitorWithThis()
        {
            var target = new ConstantExpr(1357);
            var visitor = new Mock<IExpressionVisitor<string, int>>();

            target.Accept(visitor.Object, 0);

            visitor.Verify(x => x.Visit(target, 0), Times.Once);
        }

        [Test]
        public void AcceptMethodCallsOnlyVisitOnVisitorWithThisAndNoOtherVisitMethods()
        {
            var target = new ConstantExpr(1357);
            // throw exception is any other methods called other than the ConstantExpr overload.
            var visitor = new Mock<IExpressionVisitor<string, int>>(MockBehavior.Strict);
            visitor.Setup(x => x.Visit(It.IsAny<ConstantExpr>(), It.IsAny<int>())).Returns("");

            target.Accept(visitor.Object, 0);
        }
    }
}
