using System;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Function;
using AST.Expressions.Logical;
using AST.Statements;
using AST.Visitor;
using Moq;
using NUnit.Framework;
using TestHelpers;

namespace AST.Tests.Expressions.Function
{
    namespace LambdaDefinitionExprTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase(false, false, false, false, ExpectedException = typeof (ArgumentNullException))]
            [TestCase(false, true, true, true, ExpectedException = typeof(ArgumentNullException))]
            [TestCase(true, false, true, true, ExpectedException = typeof(ArgumentNullException))]
            [TestCase(true, true, false, true, ExpectedException = typeof(ArgumentNullException))]
            [TestCase(true, true, true, false, ExpectedException = typeof(ArgumentNullException))]
            [TestCase(true, true, true, true)]
            public void ThrowsOnNulls(bool nameSupplied, bool argumentsSupplied, bool bodySupplied, bool returnTypeSupplied)
            {
                var name = nameSupplied ? new IdentifierExpr("TEST") : null;
                VarDefinitionStmt [] arguments = argumentsSupplied ? new VarDefinitionStmt [0] : null;
                var body = bodySupplied ? new ConstantExpr(RandomGenerator.Int()) : null;
                var returnType = returnTypeSupplied ? new IdentifierExpr("INT") : null;

                new LambdaDefinitionExpr(name, arguments, body, returnType);
            }
        }

        [TestFixture]
        public class Properties
        {
            [Test]
            public void FunctionNamePropertyReturnsPassedName()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new VarDefinitionStmt[0];
                var body = new ConstantExpr(RandomGenerator.Int());
                var returnType = new IdentifierExpr(RandomGenerator.String());

                var target = new LambdaDefinitionExpr(name, arguments, body, returnType);

                Assert.AreSame(name.Name, target.Name);
            }

            [Test]
            public void ArgumentsPropertyReturnsPassedArguments()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new VarDefinitionStmt[0];
                var body = new ConstantExpr(RandomGenerator.Int());
                var returnType = new IdentifierExpr(RandomGenerator.String());

                var target = new LambdaDefinitionExpr(name, arguments, body, returnType);

                Assert.AreSame(arguments, target.Arguments);
            }

            [Test]
            public void BodyPropertyReturnsPassedBody()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new VarDefinitionStmt[0];
                var body = new ConstantExpr(RandomGenerator.Int());
                var returnType = new IdentifierExpr(RandomGenerator.String());

                var target = new LambdaDefinitionExpr(name, arguments, body, returnType);

                Assert.AreSame(body, target.Body);
            }
            
            [Test]
            public void ReturnTypePropertyReturnsPassedReturnType()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new VarDefinitionStmt[0];
                var body = new ConstantExpr(RandomGenerator.Int());
                var returnType = new IdentifierExpr(RandomGenerator.String());

                var target = new LambdaDefinitionExpr(name, arguments, body, returnType);

                Assert.AreSame(returnType, target.ReturnType);
            }
        }

        [TestFixture]
        public class ConstantExprVisitorTests
        {
            [Test]
            public void AcceptMethodCallsVisitOnVisitorWithThis()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new VarDefinitionStmt[0];
                var body = new ConstantExpr(RandomGenerator.Int());
                var returnType = new IdentifierExpr(RandomGenerator.String());

                var target = new LambdaDefinitionExpr(name, arguments, body, returnType);

                var visitor = new Mock<IExpressionVisitor<string, int>>();
               
                target.Accept(visitor.Object, 0);

                visitor.Verify(x => x.Visit(target, 0), Times.Once);                
            }

            [Test]
            public void AcceptMethodCallsOnlyVisitOnVisitorWithThisAndNoOtherVisitMethods()
            {
                var name = new IdentifierExpr(RandomGenerator.String());
                var arguments = new VarDefinitionStmt[0];
                var body = new ConstantExpr(RandomGenerator.Int());
                var returnType = new IdentifierExpr(RandomGenerator.String());

                var target = new LambdaDefinitionExpr(name, arguments, body, returnType);

                // throw exception is any other methods called other than the PlusExpr overload.
                var visitor = new Mock<IExpressionVisitor<string, int>>(MockBehavior.Strict);
                visitor.Setup(x => x.Visit(target, 12345)).Returns("");

                target.Accept(visitor.Object, 12345);                
            }
        }
    }
}
