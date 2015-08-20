using System;
using System.Collections.Generic;
using AST.Expressions.Arithmatic;
using Lexer;
using Lexer.Tokeniser;
using NUnit.Framework;

namespace Parser.Tests
{
    namespace ExpressionParserTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase(ExpectedException = typeof(ArgumentNullException))]
            public void ThrowsOnNullLexer()
            {
                new ExpressionParser(null);
            }
        }

        [TestFixture]
        public class TestClass
        {
            [Test]
            public void IntegersAreParsedToAConstExpression()
            {
                var parser = new ExpressionParser(new FakeScanner(new [] { new Token("INTEGER", "1", 0, 0, 0) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(ConstantExpr), result.GetType());
            }
        
            [Test]
            public void FloatsAreParsedToAConstExpression()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { new Token("FLOAT", "2.34", 0, 0, 0) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(ConstantExpr), result.GetType());
            }

            private Token Get(object value)
            {
                var typeToToken = new Dictionary<TypeCode, string>
                {
                    {TypeCode.Double, "FLOAT"},
                    {TypeCode.Single, "FLOAT"},
                    {TypeCode.Int16, "INTEGER"},
                    {TypeCode.Int32, "INTEGER"},
                    {TypeCode.Int64, "INTEGER"}
                };

                var typeCode = Type.GetTypeCode(value.GetType());
                if (typeToToken.ContainsKey(typeCode))
                    return new Token(typeToToken[typeCode], value.ToString(), 0, 0, 0);

                var symbolToOp = new Dictionary<string, string>
                {
                    {"+", "PLUS"},
                    {"-", "MINUS"},
                    {"*", "MULT"},
                    {"/", "DIV"}
                };

                return new Token(symbolToOp[value.ToString()], value.ToString(), 0, 0, 0);
            }

            [Test]
            public void SimpleAddition()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(2), Get("+"), Get(3)}));

                var result = parser.Parse();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());

                var expr = (PlusExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(2, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(3, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void SimpleMultiplication()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(4), Get("*"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(MultExpr), result.GetType());

                var expr = (MultExpr) result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(4, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(5, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void SimpleSubtraction()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(6), Get("-"), Get(3) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(MinusExpr), result.GetType());

                var expr = (MinusExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(6, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(3, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void SimpleDivision()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(10), Get("/"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(DivExpr), result.GetType());

                var expr = (DivExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(10, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(5, ((ConstantExpr)expr.Right).Value);
            }
        }
    }
}
