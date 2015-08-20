using System;
using System.Collections.Generic;
using AST;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;
using Lexer;
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
        public class ExpressionParserStructure
        {
            [Test]
            public void ImplementsIParser()
            {
                Assert.IsTrue(typeof(IParser).IsAssignableFrom(typeof(ExpressionParser)));
            }

            [Test]
            public void ImplementsParse()
            {                
                IParser target = new ExpressionParser(new FakeScanner(new []{ Token.Empty }));
                try
                {
                    IExpression actual = target.Parse();
                    Assert.Fail("No tokens to parse");
                }
                catch (ParseException)
                {                    
                }                
            }

            [Test]
            public void ImplementsParseWithPrecedence()
            {
                IParser target = new ExpressionParser(new FakeScanner(new[] { Token.Empty }));
                try
                {
                    IExpression actual = target.Parse(0);
                    Assert.Fail("No tokens to parse");
                }
                catch (ParseException)
                {
                }          
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
                    {TypeCode.Int64, "INTEGER"},
                    {TypeCode.Boolean, "BOOLEAN"}
                };

                var typeCode = Type.GetTypeCode(value.GetType());
                if (typeToToken.ContainsKey(typeCode))
                    return new Token(typeToToken[typeCode], value.ToString(), 0, 0, 0);

                var symbolToOp = new Dictionary<string, string>
                {
                    {"+", "PLUS"},
                    {"-", "MINUS"},
                    {"*", "MULT"},
                    {"/", "DIV"},
                    {"^", "POW"},
                    {"(", "LEFTPAREN"},
                    {")", "RIGHTPAREN"},
                    {"==", "EQUALS"},
                    {"!=", "NOTEQUALS"},
                    {"<", "LT"},
                    {">", "GT"},
                    {"<=", "LTE"},
                    {">=", "GTE"},
                    {"&&", "BOOLEAN-AND"},
                    {"||", "BOOLEAN-OR"},
                    {"!", "BOOLEAN-NOT"}
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

            [Test]
            public void SimplePower()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("^"), Get(2) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(PowExpr), result.GetType());

                var expr = (PowExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(3, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(2, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void PlusMinusOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("+"), Get(2), Get("-"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(MinusExpr), result.GetType());
            }

            [Test]
            public void MinusPlusOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("-"), Get(2), Get("+"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void MultPlusOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("*"), Get(2), Get("+"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void PlusMultOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("+"), Get(2), Get("*"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void DivPlusOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("/"), Get(2), Get("+"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void PlusDivOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("+"), Get(2), Get("/"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void DivPowOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("/"), Get(2), Get("^"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(DivExpr), result.GetType());
            }

            [Test]
            public void PowDivOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("^"), Get(2), Get("/"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(DivExpr), result.GetType());
            }

            [Test]
            public void SimpleParenthesis()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get("("), Get(1), Get(")") }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(ConstantExpr), result.GetType());

                var expr = (ConstantExpr) result;

                Assert.AreEqual(1, expr.Value);
            }

            [Test]
            public void ParenthesisOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(3), Get("^"), Get("("), Get(2), Get("*"), Get(5), Get(")") }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(PowExpr), result.GetType());
            }

            [Test]
            public void ParenthesisDefalutOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get("("), Get(3), Get("^"), Get(2), Get(")"), Get("*"), Get(5) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(MultExpr), result.GetType());
            }

            [Test]
            public void NestedParenthesisOperatorPrecedence()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get("("), Get(2), Get("^"), Get("("), Get(3), Get("*"), Get(4), Get(")"), Get(")") }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(PowExpr), result.GetType());
            }

            [Test]
            public void DefaultOperatorPrecedenceAsCounterExampleToAbove()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(2), Get("^"), Get(3), Get("*"), Get(4) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(MultExpr), result.GetType());
            }

            [Test]
            public void EqualityTest()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(2), Get("=="), Get(3) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(EqualsExpr), result.GetType());

                var expr = (EqualsExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(2, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(3, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void NotEqualsTest()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(2), Get("!="), Get(3) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(NotEqualsExpr), result.GetType());

                var expr = (NotEqualsExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(2, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(3, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void GreaterThanTest()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(2), Get(">"), Get(3) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(GreaterThanExpr), result.GetType());

                var expr = (GreaterThanExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(2, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(3, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void GreaterThanOrEqualToTest()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(2), Get(">="), Get(3) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(GreaterThanOrEqualsExpr), result.GetType());

                var expr = (GreaterThanOrEqualsExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(2, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(3, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void LessThanTest()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(2), Get("<"), Get(3) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(LessThanExpr), result.GetType());

                var expr = (LessThanExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(2, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(3, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void LessThanOrEqualToTest()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(2), Get("<="), Get(3) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(LessThanOrEqualsExpr), result.GetType());

                var expr = (LessThanOrEqualsExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(2, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(3, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void LogicalAndTest()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(true), Get("&&"), Get(false) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(AndExpr), result.GetType());

                var expr = (AndExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(true, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(false, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void LogicalOrTest()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get(true), Get("||"), Get(false) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(OrExpr), result.GetType());

                var expr = (OrExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(true, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(false, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void LogicalNotTest()
            {
                var parser = new ExpressionParser(new FakeScanner(new[] { Get("!"), Get(false) }));

                var result = parser.Parse();

                Assert.AreEqual(typeof(NotExpr), result.GetType());

                var expr = (NotExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());
                Assert.AreEqual(false, ((ConstantExpr)expr.Right).Value);
            }
        }
    }
}
