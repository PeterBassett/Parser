using AST.Expressions.Arithmatic;
using AST.Expressions.Function;
using AST.Visitor;
using Moq;
using NUnit.Framework;

namespace AST.Tests.Statements
{
    [TestFixture]
    public class ReturnStmtTests
    {
        [TestCase(1)]
        [TestCase(1.2)]
        [TestCase("test")]
        [TestCase(true)]
        public void ConstructorTakesAnExpression(object value)
        {
            new ReturnStmt(new ConstantExpr(value));
        }

        [TestCase(1)]
        [TestCase(1.2)]
        [TestCase("test")]
        [TestCase(true)]
        public void ValueExpressionIsAvailableFromTheReturnExpressionProperty(object value)
        {
            var target = new ReturnStmt(new ConstantExpr(value));

            Assert.AreSame(value, ((ConstantExpr)target.ReturnExpression).Value);
        }
    }

    [TestFixture]
    public class ReturnStmtVisitorTests
    {
        [Test]
        public void AcceptMethodCallsVisitOnVisitorWithThis()
        {
            var target = new ReturnStmt(new ConstantExpr(8484));
            var visitor = new Mock<IExpressionVisitor<string, int>>();

            target.Accept(visitor.Object, 0);

            visitor.Verify(x => x.Visit(target, 0), Times.Once);
        }

        [Test]
        public void AcceptMethodCallsOnlyVisitOnVisitorWithThisAndNoOtherVisitMethods()
        {
            var target = new ReturnStmt(new ConstantExpr(1357));
            // throw exception is any other methods called other than the ReturnStmt overload.
            var visitor = new Mock<IExpressionVisitor<string, int>>(MockBehavior.Strict);
            visitor.Setup(x => x.Visit(target, 0)).Returns("");

            target.Accept(visitor.Object, 0);
        }
    }
}
