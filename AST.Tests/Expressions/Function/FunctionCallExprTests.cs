using System;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Function;
using AST.Visitor;
using Moq;
using NUnit.Framework;
using TestHelpers;

namespace AST.Tests.Expressions.Function
{
    namespace FunctionCallExprTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase(false, false, ExpectedException = typeof (ArgumentNullException))]
            [TestCase(true, false, ExpectedException = typeof (ArgumentNullException))]
            [TestCase(false, true, ExpectedException = typeof (ArgumentNullException))]
            [TestCase(true, true)]
            public void ThrowsOnNulls(bool nameSupplied, bool argumentsSupplied)
            {
                var name = nameSupplied ? new IdentifierExpr("TEST") : null;
                var arguments = argumentsSupplied ? new Expression[] { new ConstantExpr(1), new ConstantExpr(2) } : null;

                new FunctionCallExpr(name, arguments);
            }
        }

        [TestFixture]
        public class Properties
        {
            [Test]
            public void ArgumentsPropertyReturnsPassedArguments()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new Expression[] { new ConstantExpr(RandomGenerator.Int()), new ConstantExpr(RandomGenerator.Int()) };

                var target = new FunctionCallExpr(name, arguments);

                Assert.AreSame(arguments, target.Arguments);
            }

            [Test]
            public void FunctionNamePropertyReturnsPassedName()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new Expression[] { new ConstantExpr(RandomGenerator.Int()), new ConstantExpr(RandomGenerator.Int()) };

                var target = new FunctionCallExpr(name, arguments);

                Assert.AreSame(name.Name, target.FunctionName.Name);
            }
        }

        [TestFixture]
        public class ConstantExprVisitorTests
        {
            [Test]
            public void AcceptMethodCallsVisitOnVisitorWithThis()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new Expression[] { new ConstantExpr(RandomGenerator.Int()), new ConstantExpr(RandomGenerator.Int()) };

                var target = new FunctionCallExpr(name, arguments);
                var visitor = new Mock<IExpressionVisitor<string, int>>();
               
                target.Accept(visitor.Object, 0);

                visitor.Verify(x => x.Visit(target, 0), Times.Once);                
            }

            [Test]
            public void AcceptMethodCallsOnlyVisitOnVisitorWithThisAndNoOtherVisitMethods()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new Expression[] { new ConstantExpr(RandomGenerator.Int()), new ConstantExpr(RandomGenerator.Int()) };

                var target = new FunctionCallExpr(name, arguments);
                // throw exception is any other methods called other than the PlusExpr overload.
                var visitor = new Mock<IExpressionVisitor<string, int>>(MockBehavior.Strict);
                visitor.Setup(x => x.Visit(target, 123)).Returns("");

                target.Accept(visitor.Object, 123);                
            }
        }
    }
}
