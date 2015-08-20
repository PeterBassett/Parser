using System;
using System.Linq;
using Lexer;
using NUnit.Framework;

namespace Parser.Tests
{
    [TestFixture]
    public class FakeScannerTests
    {
        [TestCase(ExpectedException = typeof(ArgumentNullException))]
        public void ConstructorThrowOnNull()
        {
            new FakeScanner(null);
        }

        [Test]
        public void ConstructorAcceptsEmptyTokenList()
        {
            new FakeScanner(new Token[] {});
        }        

        [Test]
        public void ConstructorAcceptsTokenList()
        {
            new FakeScanner(new Token[] { new Token("INTEGER", "1", 0, 0, 0) });
        }

        private ILexer GetScanner(int tokensRequired)
        {
            return new FakeScanner(FakeScanner.ShortExpression.Take(tokensRequired).ToArray());
        }

        private ILexer GetScanner()
        {
            return GetScanner(1);
        }


        [Test]
        public void CurrentPropertyIsInitiallyEmpty()
        {
            var target = GetScanner();

            Assert.AreEqual(Token.Empty, target.Current);
        }

        [Test]
        public void AdvanceReturnsTrueWhenTokenAvailable()
        {
            var target = GetScanner();
            var result = target.Advance();
            Assert.IsTrue(result);
        }

        [Test]
        public void CurrentPropertyIsNotNullAfterAdvanceCalledOnce()
        {
            var target = GetScanner();
            target.Advance();

            Assert.IsNotNull(target.Current.Type);
        }

        [Test]
        public void AdvanceReturnsFalseWhenTokenStreamExhausted()
        {
            var target = GetScanner();
            var result = target.Advance();
            Assert.IsTrue(result);

            result = target.Advance();
            Assert.IsFalse(result);
        }

        [Test]
        public void AdvanceReturnsTrueWhenTokensAvailable()
        {
            int tokens = 3;
            var target = GetScanner(tokens);
            
            for (int i = 0; i < tokens + 1; i++)
            {
                var result = target.Advance();
                Assert.AreEqual(i < tokens, result);
            }
        }

        [Test]
        public void WhenAdvanceReturnsTrueCurrentIsAvailable()
        {
            int tokens = 3;
            var target = GetScanner(tokens);
            bool result;

            for (int i = 0; i < tokens; i++)
            {
                result = target.Advance();
                Assert.AreEqual(true, result);

                Assert.IsFalse(string.IsNullOrEmpty(target.Current.Type));
            }
            result = target.Advance();
            Assert.AreEqual(false, result);

            Assert.AreEqual(Token.Empty, target.Current);
        }
    }
}
