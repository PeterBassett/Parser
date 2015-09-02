using System;
using System.Collections.Generic;
using System.Linq;
using AST;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Function;
using AST.Expressions.Logical;
using AST.Statements;
using AST.Statements.Loops;
using Lexer;
using NUnit.Framework;

namespace Parser.Tests
{
    namespace StatementParserTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase(ExpectedException = typeof(ArgumentNullException))]
            public void ThrowsOnNullLexer()
            {
                new StatementParser(null);
            }
        }

        [TestFixture]
        public class StatementParserStructure
        {
            [Test]
            public void ImplementsIParser()
            {
                Assert.IsTrue(typeof(IParser).IsAssignableFrom(typeof(StatementParser)));
            }

            [Test]
            public void ImplementsParse()
            {
                IParser target = new StatementParser(new FakeScanner(new[] { Token.Empty }));
                try
                {
                    IExpression actual = target.ParseAll();
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
                var parser = new StatementParser(new FakeScanner(new[] { new Token("INTEGER", "1", 0, 0, 0) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(ConstantExpr), result.GetType());
            }

            [Test]
            public void FloatsAreParsedToAConstExpression()
            {
                var parser = new StatementParser(new FakeScanner(new[] { new Token("FLOAT", "2.34", 0, 0, 0) }));

                var result = parser.ParseAll();

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
                    {"a", "IDENTIFIER"},
                    {"b", "IDENTIFIER"},
                    {"c", "IDENTIFIER"},
                    {"d", "IDENTIFIER"},
                    {"n", "IDENTIFIER"},                    
                    {"Test", "IDENTIFIER"},
                    {"Test2", "IDENTIFIER"},
                    {"Multiply", "IDENTIFIER"}, 
                    {"Square", "IDENTIFIER"}, 
                    {"fibonacci", "IDENTIFIER"},  
                    {"int", "IDENTIFIER"},
                    {"string", "IDENTIFIER"},
                    {"class", "CLASS"},
                    {"public", "PUBLIC"},
                    {"private", "PRIVATE"},
                    {"protected", "PROTECTED"},
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
                    {"!", "BOOLEAN-NOT"},
                    {"?", "QUESTIONMARK"},
                    {":", "COLON"},
                    {"{", "LEFTBRACE"},
                    {"}", "RIGHTBRACE"},
                    {";", "SEMICOLON"},
                    {"while", "WHILE"},
                    {"=", "ASSIGNMENT"},
                    {"if", "IF"},
                    {"else", "ELSE"},
                    {"return", "RETURN"},
                    {"function", "FUNCTION"},
                    {"=>", "RIGHTARROW"},
                    {",", "COMMA"},
                    {"val", "VAL"},
                    {"var", "VAR"},
                };

                if(!symbolToOp.ContainsKey(value.ToString()))
                    throw new ArgumentException("Token dictionary doesnt contain " + value.ToString());

                return new Token(symbolToOp[value.ToString()], value.ToString(), 0, 0, 0);
            }

            [Test]
            public void SimpleAddition()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(2), Get("+"), Get(3) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(4), Get("*"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(MultExpr), result.GetType());

                var expr = (MultExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());

                Assert.AreEqual(4, ((ConstantExpr)expr.Left).Value);
                Assert.AreEqual(5, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void SimpleSubtraction()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(6), Get("-"), Get(3) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(10), Get("/"), Get(5) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("^"), Get(2) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("+"), Get(2), Get("-"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(MinusExpr), result.GetType());
            }

            [Test]
            public void MinusPlusOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("-"), Get(2), Get("+"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void MultPlusOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("*"), Get(2), Get("+"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void PlusMultOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("+"), Get(2), Get("*"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void DivPlusOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("/"), Get(2), Get("+"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void PlusDivOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("+"), Get(2), Get("/"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(PlusExpr), result.GetType());
            }

            [Test]
            public void DivPowOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("/"), Get(2), Get("^"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(DivExpr), result.GetType());
            }

            [Test]
            public void PowDivOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("^"), Get(2), Get("/"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(DivExpr), result.GetType());
            }

            [Test]
            public void SimpleParenthesis()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("("), Get(1), Get(")") }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(ConstantExpr), result.GetType());

                var expr = (ConstantExpr)result;

                Assert.AreEqual(1, expr.Value);
            }

            [Test]
            public void ParenthesisOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(3), Get("^"), Get("("), Get(2), Get("*"), Get(5), Get(")") }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(PowExpr), result.GetType());
            }

            [Test]
            public void ParenthesisDefalutOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("("), Get(3), Get("^"), Get(2), Get(")"), Get("*"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(MultExpr), result.GetType());
            }

            [Test]
            public void NestedParenthesisOperatorPrecedence()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("("), Get(2), Get("^"), Get("("), Get(3), Get("*"), Get(4), Get(")"), Get(")") }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(PowExpr), result.GetType());
            }

            [Test]
            public void DefaultOperatorPrecedenceAsCounterExampleToAbove()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(2), Get("^"), Get(3), Get("*"), Get(4) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(MultExpr), result.GetType());
            }

            [Test]
            public void EqualityTest()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(2), Get("=="), Get(3) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(2), Get("!="), Get(3) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(2), Get(">"), Get(3) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(2), Get(">="), Get(3) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(2), Get("<"), Get(3) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(2), Get("<="), Get(3) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("&&"), Get(false) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("||"), Get(false) }));

                var result = parser.ParseAll();

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
                var parser = new StatementParser(new FakeScanner(new[] { Get("!"), Get(false) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(NotExpr), result.GetType());

                var expr = (NotExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());
                Assert.AreEqual(false, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void LogicalNotPrecedenceTest()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("&&"), Get("!"), Get(false) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(AndExpr), result.GetType());

                var expr = (AndExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(NotExpr), expr.Right.GetType());
            }

            [Test]
            public void ConditionalExpressionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("?"), Get(1234), Get(":"), Get(5678) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(ConditionalExpr), result.GetType());

                var expr = (ConditionalExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Condition.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.ThenExpression.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.ElseExpression.GetType());

                Assert.AreEqual(true, ((ConstantExpr)expr.Condition).Value);
                Assert.AreEqual(1234, ((ConstantExpr)expr.ThenExpression).Value);
                Assert.AreEqual(5678, ((ConstantExpr)expr.ElseExpression).Value);
            }

            [Test]
            public void SubExpressionInConditionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("&&"), Get(false), Get("?"), Get(1234), Get(":"), Get(5678) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(ConditionalExpr), result.GetType());

                var expr = (ConditionalExpr)result;

                Assert.AreEqual(typeof(AndExpr), expr.Condition.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.ThenExpression.GetType());
                Assert.AreEqual(typeof(ConstantExpr), expr.ElseExpression.GetType());
            }

            [Test]
            public void SubExpressionInAllPositionsTest()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("&&"), Get(false), Get("?"), Get(1234), Get("+"), Get(2345), Get(":"), Get(3), Get("^"), Get(2) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(ConditionalExpr), result.GetType());

                var expr = (ConditionalExpr)result;

                Assert.AreEqual(typeof(AndExpr), expr.Condition.GetType());
                Assert.AreEqual(typeof(PlusExpr), expr.ThenExpression.GetType());
                Assert.AreEqual(typeof(PowExpr), expr.ElseExpression.GetType());
            }

            [Test]
            public void NegationExpressionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("-"), Get(5) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(NegationExpr), result.GetType());

                var expr = (NegationExpr)result;

                Assert.AreEqual(typeof(ConstantExpr), expr.Right.GetType());
                Assert.AreEqual(5, ((ConstantExpr)expr.Right).Value);
            }

            [Test]
            public void AndOrPrecedenceTest1()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("||"), Get(true), Get("&&"), Get(false) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(OrExpr), result.GetType());
            }

            [Test]
            public void AndOrPrecedenceTest2()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("("), Get(true), Get("||"), Get(true), Get(")"), Get("&&"), Get(false) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(AndExpr), result.GetType());
            }

            [Test]
            public void AndOrPrecedenceTest3()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("||"), Get("("), Get(true), Get("&&"), Get(false), Get(")") }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(OrExpr), result.GetType());
            }

            [Test]
            public void AndOrPrecedenceTest4()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("&&"), Get(true), Get("||"), Get(false) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(AndExpr), result.GetType());
            }

            [Test]
            public void AndOrPrecedenceTest5()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("("), Get(true), Get("&&"), Get(true), Get(")"), Get("||"), Get(false) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(OrExpr), result.GetType());
            }

            [Test]
            public void AndOrPrecedenceTest6()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get(true), Get("&&"), Get("("), Get(true), Get("||"), Get(false), Get(")") }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(AndExpr), result.GetType());
            }

            [Test]
            public void SimpleAssignment()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("a"), Get("="), Get("b"), Get("+"), Get(1) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(AssignmentExpr), result.GetType());
                var expr = (AssignmentExpr)result;

                Assert.AreEqual(typeof(IdentifierExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(PlusExpr), expr.Right.GetType());
            }

            [Test]
            public void MoreComplexAssignment()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("a"), Get("="), Get("b"), Get("+"), Get(1), Get("*"), Get(5), Get("-"), Get(4) }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(AssignmentExpr), result.GetType());

                var expr = (AssignmentExpr)result;

                Assert.AreEqual(typeof(IdentifierExpr), expr.Left.GetType());
                Assert.AreEqual(typeof(MinusExpr), expr.Right.GetType());
            }

            [Test]
            public void WhileStatementWithBlock()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("while"), Get("("), Get(1), Get("!="), Get(3), Get(")"), Get("{"), Get("a"), Get("="), Get("b"), Get("+"), Get(1), Get(";"), Get("}") }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(WhileStmt), result.GetType());

                var expr = (WhileStmt)result;

                Assert.AreEqual(typeof(NotEqualsExpr), expr.Condition.GetType());
                Assert.AreEqual(typeof(ScopeBlockStmt), expr.Block.GetType());

                var block = (ScopeBlockStmt)expr.Block;
                Assert.AreEqual(1, block.Statements.Count());
                Assert.AreEqual(typeof(AssignmentExpr), block.Statements.ElementAt(0).GetType());
            }

            [Test]
            public void WhileStatementSingle()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("while"), Get("("), Get(1), Get("!="), Get(3), Get(")"), Get("a"), Get("="), Get("b"), Get("+"), Get(1), Get(";") }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(WhileStmt), result.GetType());

                var expr = (WhileStmt)result;

                Assert.AreEqual(typeof(NotEqualsExpr), expr.Condition.GetType());
                Assert.AreEqual(typeof(AssignmentExpr), expr.Block.GetType());
            }

            [TestCase(ExpectedException=typeof(ParseException))]
            public void WhileStatementWithExpression()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("while"), Get("("), Get(1), Get("!="), Get(3), Get(")"), Get("b"), Get("+"), Get(1), Get(";") }));

                parser.ParseAll();
            }

            [TestCase(ExpectedException = typeof(ParseException))]
            public void WhileStatementWithExpressionBlock()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("while"), Get("("), Get(1), Get("!="), Get(3), Get(")"), Get("{"), Get("b"), Get("+"), Get(1), Get(";"), Get("}") }));

                parser.ParseAll();
            }

            [Test]
            public void BlockStatement()
            {
                var parser = new StatementParser(new FakeScanner(new[] { Get("{"), Get("a"), Get("="), Get("b"), Get("+"), Get(1), Get(";"), Get("}") }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(ScopeBlockStmt), result.GetType());
            }

            [Test]
            public void IfStatementWithBlock()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("if"), Get("("), Get(1), Get("!="), Get(3), Get(")"), 
                    Get("{"), 
                        Get("a"), Get("="), Get("b"), Get("+"), Get(1), Get(";"), 
                    Get("}")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(IfStmt), result.GetType());

                var stmt = (IfStmt) result;

                Assert.AreEqual(typeof(NotEqualsExpr), stmt.Condition.GetType());
                Assert.AreEqual(typeof(ScopeBlockStmt), stmt.ThenExpression.GetType());
                Assert.AreEqual(typeof(NoOpStatement), stmt.ElseExpression.GetType());
            }

            [Test]
            public void IfElseStatementWithBlock()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("if"), Get("("), Get(1), Get("!="), Get(3), Get(")"), 
                    Get("{"), 
                        Get("a"), Get("="), Get("b"), Get("+"), Get(1), Get(";"), 
                    Get("}"),
                    Get("else"),
                    Get("{"), 
                        Get("c"), Get("="), Get("d"), Get("+"), Get(2), Get(";"), 
                    Get("}")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(IfStmt), result.GetType());

                var stmt = (IfStmt)result;

                Assert.AreEqual(typeof(NotEqualsExpr), stmt.Condition.GetType());
                Assert.AreEqual(typeof(ScopeBlockStmt), stmt.ThenExpression.GetType());
                Assert.AreEqual(typeof(ScopeBlockStmt), stmt.ElseExpression.GetType());

                Assert.AreNotSame(stmt.ThenExpression, stmt.ElseExpression);
            }

            [Test]
            public void IfElseStatementNoBlock()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("if"), Get("("), Get(1), Get("!="), Get(3), Get(")"), 
                        Get("a"), Get("="), Get("b"), Get("+"), Get(1), Get(";"), 
                    Get("else"),
                        Get("c"), Get("="), Get("d"), Get("+"), Get(2), Get(";"), 
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(IfStmt), result.GetType());

                var stmt = (IfStmt)result;

                Assert.AreEqual(typeof(NotEqualsExpr), stmt.Condition.GetType());
                Assert.AreEqual(typeof(AssignmentExpr), stmt.ThenExpression.GetType());
                Assert.AreEqual(typeof(AssignmentExpr), stmt.ElseExpression.GetType());

                Assert.AreNotSame(stmt.ThenExpression, stmt.ElseExpression);
            }

            [Test]
            public void IfIfElseStatementNoBlocks()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("if"), Get("("), Get(true), Get(")"),                     
                        Get("if"), Get("("), Get(false), Get(")"), 
                            Get("a"), Get("="), Get("b"), Get("+"), Get(1), Get(";"), 
                        Get("else"),
                            Get("c"), Get("="), Get("d"), Get("+"), Get(2), Get(";"), 
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(IfStmt), result.GetType());

                var stmt = (IfStmt)result;

                Assert.AreEqual(typeof(ConstantExpr), stmt.Condition.GetType());
                Assert.AreEqual(true, ((ConstantExpr)stmt.Condition).Value);
                
                Assert.AreEqual(typeof(IfStmt), stmt.ThenExpression.GetType());
                Assert.AreEqual(typeof(NoOpStatement), stmt.ElseExpression.GetType());

                var innerIf = (IfStmt)stmt.ThenExpression;
                Assert.AreEqual(typeof(ConstantExpr), innerIf.Condition.GetType());
                Assert.AreEqual(false, ((ConstantExpr)innerIf.Condition).Value);                
            }

            [Test]
            public void FunctionDefinitionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("function"), Get("Test"), Get("("),Get(")"),
                    Get("{"),
                        Get("return"), Get(1), Get(";"),
                    Get("}")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(FunctionDefinitionExpr), result.GetType());

                var stmt = (FunctionDefinitionExpr)result;

                Assert.AreEqual("Test", stmt.Name);
                Assert.AreEqual(typeof(ScopeBlockStmt), stmt.Body.GetType());
            }

            [Test]
            public void FunctionCallTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("Test"), Get("("), Get(")"), Get(";"),
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(FunctionCallExpr), result.GetType());

                var stmt = (FunctionCallExpr)result;

                Assert.AreEqual("Test", stmt.FunctionName.Name);
                Assert.AreEqual(0, stmt.Arguments.Count());
            }

            [Test]
            public void FunctionCallWithParameterTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("Test"), Get("("), Get(1), Get(")"), Get(";"),
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(FunctionCallExpr), result.GetType());

                var stmt = (FunctionCallExpr)result;

                Assert.AreEqual("Test", stmt.FunctionName.Name);
                Assert.AreEqual(1, stmt.Arguments.Count());
                var argument = (ConstantExpr)stmt.Arguments.ElementAt(0);
                Assert.AreEqual(1, argument.Value);
            }

            [Test]
            public void FunctionCallWithExpressionParameterTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("Test"), Get("("), Get(1), Get("+"), Get(2), Get("*"), Get(5), Get(")"), Get(";"),
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(FunctionCallExpr), result.GetType());

                var stmt = (FunctionCallExpr)result;

                Assert.AreEqual("Test", stmt.FunctionName.Name);
                Assert.AreEqual(1, stmt.Arguments.Count());
                Assert.AreEqual(typeof(PlusExpr), stmt.Arguments.ElementAt(0).GetType());
            }

            [Test]
            public void FunctionCallWithMultipleParametersTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("Test"), Get("("), Get(1), Get(","), Get(2), Get(","), Get(3), Get(")"), Get(";"),
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(FunctionCallExpr), result.GetType());

                var stmt = (FunctionCallExpr)result;

                Assert.AreEqual("Test", stmt.FunctionName.Name);
                Assert.AreEqual(3, stmt.Arguments.Count());
                
                Assert.AreEqual(1, ((ConstantExpr)stmt.Arguments.ElementAt(0)).Value);
                Assert.AreEqual(2, ((ConstantExpr)stmt.Arguments.ElementAt(1)).Value);
                Assert.AreEqual(3, ((ConstantExpr)stmt.Arguments.ElementAt(2)).Value);
            }

            [Test]
            public void FunctionCallWithMultipleExpressionParametersTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("Test"), Get("("), Get(1), Get("+"), Get(2), Get("*"), Get(5), Get(","), Get(1), Get("*"), Get(2), Get(")"), Get(";"),
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(FunctionCallExpr), result.GetType());

                var stmt = (FunctionCallExpr)result;

                Assert.AreEqual("Test", stmt.FunctionName.Name);
                Assert.AreEqual(2, stmt.Arguments.Count());
                Assert.AreEqual(typeof(PlusExpr), stmt.Arguments.ElementAt(0).GetType());
                Assert.AreEqual(typeof(MultExpr), stmt.Arguments.ElementAt(1).GetType());
            }

            [Test]
            public void FunctionDefinitionWithParametersTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("function"), Get("Multiply"), Get("("), Get("a"), Get(":"), Get("int"), Get(","), Get("b"), Get(":"), Get("int"), Get(")"),
                    Get("{"),
                        Get("return"), Get("a"), Get("*"), Get("b"), Get(";"),
                    Get("}")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(FunctionDefinitionExpr), result.GetType());

                var stmt = (FunctionDefinitionExpr)result;

                Assert.AreEqual("Multiply", stmt.Name);
                Assert.AreEqual(typeof(ScopeBlockStmt), stmt.Body.GetType());
            }

            [Test]
            public void FibonacciFunctionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("function"), Get("fibonacci"), Get("("), Get("n"), Get(":"), Get("int"), Get(")"),
                    Get("{"),
                        Get("if"),Get("("),Get("n"), Get("=="), Get(0), Get(")"),
                            Get("return"), Get(0), Get(";"),
                        Get("else"), Get("if"), Get("("), Get("n"), Get("=="), Get(1), Get(")"),
                            Get("return"), Get(1), Get(";"),
                        Get("else"),
                            Get("return"), Get("fibonacci"), Get("("), Get("n"), Get("-"), Get(1), Get(")"), Get("+"), Get("fibonacci"), Get("("), Get("n"), Get("-"), Get(2), Get(")"), Get(";"),
                    Get("}")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(FunctionDefinitionExpr), result.GetType());

                var stmt = (FunctionDefinitionExpr)result;

                Assert.AreEqual("fibonacci", stmt.Name);

                Assert.AreEqual(1, stmt.Arguments.Length);
                Assert.AreEqual("n", stmt.Arguments.First().Name.Name);

                Assert.AreEqual(typeof (ScopeBlockStmt), stmt.Body.GetType());
                var body = (ScopeBlockStmt) stmt.Body;

                Assert.AreEqual(1, body.Statements.Count());

                Assert.AreEqual(typeof(IfStmt), body.Statements.First().GetType());

                var ifStmt = (IfStmt)body.Statements.First();
                Assert.AreEqual(typeof(EqualsExpr), ifStmt.Condition.GetType());
                var condition = (EqualsExpr)ifStmt.Condition;
                Assert.AreEqual("n", ((IdentifierExpr)condition.Left).Name);
                Assert.AreEqual(0, ((ConstantExpr)condition.Right).Value);

                Assert.AreEqual(typeof(ReturnStmt), ifStmt.ThenExpression.GetType());
                var returnZeroStmt = (ReturnStmt)ifStmt.ThenExpression;
                Assert.AreEqual(0, ((ConstantExpr)returnZeroStmt.ReturnExpression).Value);

                var elseNotZeroStmt = (IfStmt)ifStmt.ElseExpression;
                var equalsOneCondition = (EqualsExpr)elseNotZeroStmt.Condition;
                Assert.AreEqual("n", ((IdentifierExpr)equalsOneCondition.Left).Name);
                Assert.AreEqual(1, ((ConstantExpr)equalsOneCondition.Right).Value);     
          
                Assert.AreEqual(typeof(ReturnStmt), elseNotZeroStmt.ThenExpression.GetType());
                var returnOneStmt = (ReturnStmt)elseNotZeroStmt.ThenExpression;
                Assert.AreEqual(1, ((ConstantExpr)returnOneStmt.ReturnExpression).Value);

                var elseNotOneStmt = (ReturnStmt)elseNotZeroStmt.ElseExpression;
                var returnFibExpr = (PlusExpr)elseNotOneStmt.ReturnExpression;
                Assert.AreEqual(typeof(FunctionCallExpr), returnFibExpr.Left.GetType());
                Assert.AreEqual(typeof(FunctionCallExpr), returnFibExpr.Right.GetType());

                Assert.AreEqual("fibonacci",  ((FunctionCallExpr)returnFibExpr.Left).FunctionName.Name);
                Assert.AreEqual("fibonacci", ((FunctionCallExpr)returnFibExpr.Right).FunctionName.Name);

                var fibMinusOneCall = ((FunctionCallExpr) returnFibExpr.Left);
                var fibMinusTwoCall = ((FunctionCallExpr)returnFibExpr.Right);

                Assert.AreEqual(1, fibMinusOneCall.Arguments.Count());
                Assert.AreEqual(1, fibMinusTwoCall.Arguments.Count());

                Assert.AreEqual(typeof(MinusExpr), fibMinusOneCall.Arguments.First().GetType());
                Assert.AreEqual(typeof(MinusExpr), fibMinusTwoCall.Arguments.First().GetType());

                var minusOneExpr = (MinusExpr)fibMinusOneCall.Arguments.First();
                var minusTwoExpr = (MinusExpr)fibMinusTwoCall.Arguments.First();

                Assert.AreEqual("n", ((IdentifierExpr)minusOneExpr.Left).Name);
                Assert.AreEqual("n", ((IdentifierExpr)minusTwoExpr.Left).Name);

                Assert.AreEqual(1, ((ConstantExpr)minusOneExpr.Right).Value);
                Assert.AreEqual(2, ((ConstantExpr)minusTwoExpr.Right).Value);
            }
            [Test]
            public void MultiStatementFunctionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("function"), Get("Test"), Get("("), Get(")"),
                    Get("{"),
                        Get("var"), Get("a"),Get("="), Get(0), Get(";"),
                        Get("var"), Get("b"),Get("="), Get(1), Get(";"),
                        
                        Get("return"), Get("a"), Get("=="), Get("b"), Get(";"),
                    Get("}")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(FunctionDefinitionExpr), result.GetType());

                var stmt = (FunctionDefinitionExpr)result;

                Assert.AreEqual("Test", stmt.Name);

                Assert.AreEqual(0, stmt.Arguments.Length);

                Assert.AreEqual(typeof(ScopeBlockStmt), stmt.Body.GetType());
                var body = (ScopeBlockStmt)stmt.Body;

                Assert.AreEqual(3, body.Statements.Count());

                Assert.AreEqual(typeof(VarDefinitionStmt), body.Statements.ElementAt(0).GetType());
                Assert.AreEqual(typeof(VarDefinitionStmt), body.Statements.ElementAt(1).GetType());
                Assert.AreEqual(typeof(ReturnStmt), body.Statements.ElementAt(2).GetType());
            }

            [Test]
            public void LambdaDefinitionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("function"), Get("Test"), Get("("),Get(")"), Get("=>"), Get(1), Get(";") 
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(LambdaDefinitionExpr), result.GetType());

                var stmt = (LambdaDefinitionExpr)result;

                Assert.AreEqual("Test", stmt.Name);
                Assert.AreEqual(typeof(ConstantExpr), stmt.Body.GetType());
                Assert.AreEqual(1, ((ConstantExpr)stmt.Body).Value);
            }

            [Test]
            public void AnonymousLambdaDefinitionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("function"), Get("("),Get(")"), Get("=>"), Get(1), Get(";") 
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(LambdaDefinitionExpr), result.GetType());

                var stmt = (LambdaDefinitionExpr)result;

                Assert.AreEqual(null, stmt.Name);
                Assert.AreEqual(typeof(ConstantExpr), stmt.Body.GetType());
                Assert.AreEqual(1, ((ConstantExpr)stmt.Body).Value);
            }

            [Test]
            public void MoreComplexLambdaDefinitionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("function"), Get("Test"), Get("("),Get(")"), Get("=>"), Get(1), Get("*"), Get(2), Get(";") 
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(LambdaDefinitionExpr), result.GetType());

                var stmt = (LambdaDefinitionExpr)result;

                Assert.AreEqual("Test", stmt.Name);
                Assert.AreEqual(typeof(MultExpr), stmt.Body.GetType());                
            }

            [Test]
            public void LambdaDefinitionAndCallTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("function"), Get("Square"), Get("("),Get("n"), Get(":"), Get("int"), Get(")"), Get("=>"), Get("n"), Get("*"), Get(2), Get(";"),
                    Get("val"), Get("Test"), Get(":"), Get("int"), Get("="), Get("Square"), Get("("), Get(2), Get(")"), Get(";")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(StatementList), result.GetType());

                var stmtList = (StatementList)result;
                var funcDecl = (LambdaDefinitionExpr)stmtList.Statements.ElementAt(0);
                var varDecl = (VarDefinitionStmt)stmtList.Statements.ElementAt(1);

                Assert.AreEqual("Square", funcDecl.Name);
                Assert.AreEqual("Test", varDecl.Name.Name);
                Assert.AreEqual("Square", ((FunctionCallExpr)varDecl.InitialValue).FunctionName.Name);
            }

            [Test]
            public void ValDeclarationTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("val"), Get("Test"), Get(":"), Get("int"), Get("="), Get(1), Get(";")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(VarDefinitionStmt), result.GetType());

                var stmt = (VarDefinitionStmt)result;

                Assert.AreEqual("Test", stmt.Name.Name);
                Assert.AreEqual("int", stmt.Type.Name);
                Assert.AreEqual(true, stmt.IsConst);
                Assert.AreEqual(typeof(ConstantExpr), stmt.InitialValue.GetType());
            }

            [TestCase(ExpectedException=typeof(ParseException))]
            public void InvalidValDeclarationBecauseNoInitialiserTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("val"), Get("Test"), Get(":"), Get("int"), Get(";")
                }));

                parser.ParseAll();
            }

            [Test]
            public void MultipleValDeclarationTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("val"), Get("Test"), Get(":"), Get("int"), Get("="), Get(100), Get(";"),
                    Get("val"), Get("Test2"), Get("="), Get(10), Get(";")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(StatementList), result.GetType());

                var stmt = (StatementList)result;

                Assert.AreEqual(2, stmt.Statements.Count());

                var val1 = (VarDefinitionStmt)stmt.Statements.ElementAt(0);
                Assert.AreEqual("Test", val1.Name.Name);
                Assert.AreEqual("int", val1.Type.Name);
                Assert.AreEqual(true, val1.IsConst);
                Assert.AreEqual(100, ((ConstantExpr)val1.InitialValue).Value);

                var val2 = (VarDefinitionStmt)stmt.Statements.ElementAt(1);
                Assert.AreEqual("Test2", val2.Name.Name);
                Assert.IsNull(val2.Type.Name);
                Assert.AreEqual(true, val2.IsConst);
                Assert.AreEqual(10, ((ConstantExpr)val2.InitialValue).Value);

            }

            [Test]
            public void ValDeclarationWithInitialisationExpressionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("val"), Get("Test"), Get(":"), Get("int"), Get("="), Get(1), Get("+"), Get(2), Get(";")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(VarDefinitionStmt), result.GetType());

                var stmt = (VarDefinitionStmt)result;

                Assert.AreEqual("Test", stmt.Name.Name);
                Assert.AreEqual("int", stmt.Type.Name);
                Assert.AreEqual(true, stmt.IsConst);
                Assert.AreEqual(typeof(PlusExpr), stmt.InitialValue.GetType());
            }

            [Test]
            public void VarDeclarationTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("var"), Get("Test"), Get(":"), Get("int"), Get(";")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(VarDefinitionStmt), result.GetType());

                var stmt = (VarDefinitionStmt)result;

                Assert.AreEqual("Test", stmt.Name.Name);
                Assert.AreEqual("int", stmt.Type.Name);
                Assert.AreEqual(false, stmt.IsConst);
                Assert.IsNull(stmt.InitialValue);
            }

            [Test]
            public void VarDeclarationWithInitialisationExpressionTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("var"), Get("Test"), Get(":"), Get("int"), Get("="), Get(1), Get("+"), Get(2), Get(";")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(VarDefinitionStmt), result.GetType());

                var stmt = (VarDefinitionStmt)result;

                Assert.AreEqual("Test", stmt.Name.Name);
                Assert.AreEqual("int", stmt.Type.Name);
                Assert.AreEqual(false, stmt.IsConst);
                Assert.AreEqual(typeof(PlusExpr), stmt.InitialValue.GetType());
            }

            [Test]
            public void ClassDeclarationTest()
            {
                var parser = new StatementParser(new FakeScanner(new[]
                {
                    Get("class"), Get("Test"), Get("{"), Get("var"), Get("a"), Get(":"), Get("int"), Get("="), Get(1), Get(";"), Get("}")
                }));

                var result = parser.ParseAll();

                Assert.AreEqual(typeof(ClassDefinitionStmt), result.GetType());
            }
        }
    }
}
