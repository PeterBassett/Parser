using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Lexer.Tokeniser;

namespace Lexer.Tests.Tokeniser
{
    namespace CSRegexTokeniserTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase]
            public void TakesNoParameters()
            {
                new CSharpRegexTokeniser();
            }
        }

        [TestFixture]
        public class TokenisersProperty
        {
            private readonly CSharpRegexTokeniser _target = new CSharpRegexTokeniser();

            [TestCase]
            public void ReturnsNotNull()
            {
                Assert.IsNotNull(_target.Tokenisers);
            }

            [TestCase]
            public void ReturnsNotEmpty()
            {
                Assert.IsTrue(_target.Tokenisers.Any());
            }

            [TestCase]
            public void ReturnsIEnumerableOfRegexTokeniser()
            {
                Assert.IsTrue(_target.Tokenisers is IEnumerable<RegexTokeniser>);
            }
        }

        [TestFixture]
        public class TokenisersMatch
        {
            private readonly CSharpRegexTokeniser _target = new CSharpRegexTokeniser();

            RegexTokeniser For(string name)
            {
                return (from tokeniser in _target.Tokenisers
                        where tokeniser.Type.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                        select tokeniser).First();
            }

            [TestCase("INTEGER", "1234", "a1234")]
            [TestCase("FLOAT", "12.34", "a12.34")]
            [TestCase("COMMENT", "/* this is a test \t \r\n   \t\t */", "//")]
            [TestCase("QUOTED-STRING", "\"this is a 1234 test\"", "a")]
            public void MatchTest(string name, string shouldMatch, string shouldFail)
            {
                Assert.AreEqual(shouldMatch, For(name).Regex.Match(shouldMatch).Value);
                Assert.AreNotEqual(shouldFail, For(name).Regex.Match(shouldFail).Value);
            }
        }
    }
}
