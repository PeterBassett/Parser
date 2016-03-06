using System;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using Moq;
using NUnit.Framework;
using TestHelpers;

namespace AST.Tests.Expressions
{
    [TestFixture]
    public class ConditionalExpressionConstructorTests
    {
        [Test]
        public void ConstructorTakesThreeExpressions()
        {
            new ConditionalExpr(new ConstantExpr(true), new ConstantExpr(1), new ConstantExpr(2));
        }

        [Test]
        public void ThereIsOnlyOneConstructor()
        {
            Assert.AreEqual(1, typeof (ConditionalExpr).GetConstructors().Length);
        }

        [TestCase(true, true, true)]
        [TestCase(false, true, true, ExpectedException = typeof(ArgumentNullException))]        
        [TestCase(true, false, true, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(true, true, false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(false, true, false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(true, false, false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(false, false, true, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(true, false, false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(false, false, true, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(false, true, false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(true, false, false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(false, false, false, ExpectedException = typeof(ArgumentNullException))]        
        public void ConstructorThrowsOnNullParameters(bool conditionSupplied, bool thenExpressionSupplied, bool elseExpressionSupplied)
        {
            var condition = conditionSupplied ? new Mock<Expression>().Object : null;
            var thenExpression = thenExpressionSupplied ? new Mock<Expression>().Object : null;
            var elseExpression = elseExpressionSupplied ? new Mock<Expression>().Object : null;

            new ConditionalExpr(condition, thenExpression, elseExpression);
        }
    }

    [TestFixture]
    public class ConditionalExpressionBehaviourTests
    {
        public ConditionalExpr MakeCondition(bool condition, int valueTrue, int valueFalse)
        {
            var conditionExpression = new ConstantExpr(condition);
            var trueExpression = new ConstantExpr(valueTrue);
            var falseExpression = new ConstantExpr(valueFalse);

            return new ConditionalExpr(conditionExpression, trueExpression, falseExpression);
        }

        [Test]
        public void TrueConditionExpressionIsAvailableFromConditionProperty()
        {
            const bool expected = true;

            var target = MakeCondition(expected, 1, 2);

            Assert.AreEqual(expected, ((ConstantExpr)target.Condition).Value);
        }

        [Test]
        public void FalseConditionExpressionIsAvailableFromConditionProperty()
        {
            const bool expected = false;

            var target = MakeCondition(expected, 1, 2);

            Assert.AreEqual(expected, ((ConstantExpr)target.Condition).Value);
        }

        [Test]
        public void ThenExpressionIsAvailableFromThenProperty()
        {
            const int expected = 1;

            var target = MakeCondition(true, expected, 2);

            Assert.AreEqual(expected, ((ConstantExpr)target.ThenExpression).Value);
        }

        [Test]
        public void ThenExpressionIsAvailableFromThenPropertyRandomised()
        {
            int expected = RandomGenerator.Int();

            var target = MakeCondition(true, expected, 2);

            Assert.AreEqual(expected, ((ConstantExpr)target.ThenExpression).Value);
        }

        [Test]
        public void ElseExpressionIsAvailableFromElseProperty()
        {
            const int expected = 1;

            var target = MakeCondition(true, 3, expected);

            Assert.AreEqual(expected, ((ConstantExpr)target.ElseExpression).Value);
        }

        [Test]
        public void ElseExpressionIsAvailableFromElsePropertyRandomised()
        {
            int expected = RandomGenerator.Int();

            var target = MakeCondition(true, 3, expected);

            Assert.AreEqual(expected, ((ConstantExpr)target.ElseExpression).Value);
        }
    }
}
