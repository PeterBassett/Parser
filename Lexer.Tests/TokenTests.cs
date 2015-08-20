using System;
using System.Linq;   
using TestHelpers;
using NUnit.Framework;

namespace Lexer.Tests
{
    namespace TokenTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase]
            public void DoesNotThrowWithValidArguments()
            {
                new Token("INTEGER", "12345", 1, 1, 1);
            }

            [TestCase(null, "a", 1, 1, 1, ExpectedException = typeof(ArgumentNullException))]
            [TestCase("", "a", 1, 1, 1, ExpectedException = typeof(ArgumentOutOfRangeException))]
            [TestCase("String", "a", 1, 1, 1)]
            [TestCase("WhiteSpace", "\t  \r\n\t\t", 1, 1, 1)]
            [TestCase("Invalid", "a", 1, 1, 1)]
            [TestCase("String", null, 1, 1, 1, ExpectedException = typeof(ArgumentNullException))]
            [TestCase("String", "a", -1, 1, 1, ExpectedException = typeof(ArgumentOutOfRangeException))]
            [TestCase("String", "a", 1, -1, 1, ExpectedException = typeof(ArgumentOutOfRangeException))]
            [TestCase("String", "a", 1, 1, -1, ExpectedException = typeof(ArgumentOutOfRangeException))]
            public void TestConstructor(string token, string lexeme, int positionInStream, int charNumber, int lineNumber)
            {
                new Token(token, lexeme, positionInStream, charNumber, lineNumber);
            }
        }

        [TestFixture]
        public class Properties
        {
            private readonly string _type;
            private readonly string _lexeme;
            private readonly int _positionInStream;
            private readonly int _charNumber;
            private readonly int _lineNumber;
            private Token _target;

            public Properties()
            {
                _type = Enum.GetValues(typeof (TokenType)).Cast<TokenType>().TakeRandomSingle().ToString();
                _lexeme = RandomGenerator.String(10, 20);
                _positionInStream = RandomGenerator.Int();
                _charNumber = RandomGenerator.Int();
                _lineNumber = RandomGenerator.Int();
            }

            [SetUp]
            public void Setup()
            {
                _target = new Token(_type, _lexeme, _positionInStream, _charNumber, _lineNumber);    
            }

            [TestCase]
            public void TypeIsStored()
            {
                Assert.AreEqual(_type, _target.Type);
            }

            [TestCase]
            public void LexemeIsStored()
            {
                Assert.AreEqual(_lexeme, _target.Lexeme);
            }

            [TestCase]
            public void PositionInStreamIsStored()
            {
                Assert.AreEqual(_positionInStream, _target.PositionInStream);
            }

            [TestCase]
            public void CharNumberIsStored()
            {
                Assert.AreEqual(_charNumber, _target.CharNumber);
            }
            [TestCase]
            public void LineNumberIsStored()
            {
                Assert.AreEqual(_lineNumber, _target.LineNumber);
            }
        }
    }
}
