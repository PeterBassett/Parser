using System;
using System.Text.RegularExpressions;
using TestHelpers;
using NUnit.Framework;
using Lexer.Tokeniser;

namespace Lexer.Tests.Tokeniser
{
    namespace RegexTokeniserTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase]
            public void TakesTwoStringParameters()
            {
                new RegexTokeniser("regex", "type");
            }

            [TestCase]
            public void TakesARegexObjectAndAStringAsParameters()
            {
                new RegexTokeniser(new Regex("pattern"), "type");
            }

            [TestCase("a", "b")]
            [TestCase("", "b", ExpectedException = typeof(ArgumentOutOfRangeException))]                                                  
            [TestCase(null, "b", ExpectedException = typeof(ArgumentNullException))]
            [TestCase("a", "", ExpectedException = typeof(ArgumentOutOfRangeException))]
            [TestCase("a", null, ExpectedException = typeof(ArgumentNullException))]
            [TestCase("invalid regex +[", "valid", ExpectedException = typeof(ArgumentException))]
            public void TakesARegexObjectAndAStringAsParameters(string pattern, string type)
            {
                new RegexTokeniser(pattern, type);
            }
        }

        [TestFixture]
        public class Properties
        {
            private readonly RegexTokeniser _target = new RegexTokeniser("abc", "123");

            [TestCase]
            public void TypeIsStored()
            {
                Assert.AreEqual("123", _target.Type);
            }

            [TestCase]
            public void RegexIsNotNull()
            {
                Assert.IsNotNull(_target.Regex);
            }

            [TestCase]
            public void ReturnsIEnumerableOfRegexTokeniser()
            {
                Assert.AreEqual("abc", _target.Regex.ToString());
            }
        }

        [TestFixture]
        public class ConstructsAValidRegex
        {
            private readonly RegexTokeniser _target = new RegexTokeniser("^[0-9]+$", "NUMBERS");

            [TestCase]
            public void TypeIsStored()
            {
                Assert.AreEqual("NUMBERS", _target.Type);
            }

            [TestCase("0")]
            [TestCase("1")]
            [TestCase("2")]
            [TestCase("3")]
            [TestCase("4")]
            [TestCase("5")]
            [TestCase("6")]
            [TestCase("7")]
            [TestCase("8")]
            [TestCase("9")]
            [TestCase("01")]
            [TestCase("10")]
            [TestCase("0123456789")]
            [TestCase("0102030405060708090")]
            public void MatchesNumericCharacters(string match)
            {
                Assert.IsTrue(_target.Regex.IsMatch(match));
            }

            [TestCase]
            public void MatchesValidNumbers()
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.IsTrue(_target.Regex.IsMatch(RandomGenerator.Int().ToString()));
                }
            }

            [TestCase("a")]
            [TestCase("1a")]
            [TestCase("2$")]
            [TestCase("3^")]
            [TestCase("4i")]
            [TestCase("5(")]
            [TestCase("(6")]
            [TestCase("7!")]
            [TestCase("8@")]
            [TestCase("9#")]
            [TestCase("0102030405060708090-")]
            public void DoesNotMatchInvalidCharacters(string match)
            {
                Assert.IsFalse(_target.Regex.IsMatch(match));
            }
        }
    }
}
