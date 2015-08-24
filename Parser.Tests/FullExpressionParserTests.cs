using System;
using AST;
using AST.Visitor;
using Lexer;
using Lexer.Decorators;
using Lexer.Tokeniser;
using NUnit.Framework;

namespace Parser.Tests
{
    namespace FullExpressionParserTests
    {
        [TestFixture]
        public class FullExpressionParserTest
        {            
            public ExpressionParser SetupParser(string source)
            {
                return new ExpressionParser(new StripLexer(new Scanner(source, new CSharpRegexTokeniser())));
            }

            public IExpression CreateExpression(string source)
            {
                var target = SetupParser(source);

                return target.Parse();
            }

            [TestCase("1+2", 3)]
            [TestCase("true && false", false)]
            [TestCase("false && true", false)]
            [TestCase("true || false", true)]
            [TestCase("false || true", true)]
            [TestCase("false == true", false)]
            [TestCase("false != true", true)]
            [TestCase("true == true", true)]
            [TestCase("true != true", false)]
            [TestCase("!false", true)]
            [TestCase("!true", false)]
            [TestCase("true||true&&false", true)]
            [TestCase("true || true && false", true)]            
            [TestCase("(true||true)&&false", false)]
            [TestCase("true||(true&&false)", true)]
            [TestCase("1*2*3*4*5*6", 720)]
            [TestCase("6*5*4*3*2*1", 720)]
            [TestCase("7^5", 16807)]
            [TestCase("7^5.0", 16807.0)]
            [TestCase("7.0^5", 16807.0)]
            [TestCase("7^5/5", 3361.4)]
            [TestCase("true?1:2", 1)]
            [TestCase("false?1:2", 2)]
            [TestCase("1+2==3?\"TRUE!\":\"FALSE!\"", "TRUE!")]
            [TestCase("3.14*2==6.28?10*10:4^2", 100)]
            [TestCase("\"a\"==\"b\"", false)]
            [TestCase("\"a\"<\"b\"", true)]
            [TestCase("\"a\">\"b\"", false)]
            [TestCase("\"a\"!=\"b\"", true)]
            [TestCase("\"a\">=\"b\"", false)]
            [TestCase("\"a\"<=\"b\"", true)]
            [TestCase("\"a\"==\"a\"", true)]
            [TestCase("\"a\"<\"a\"", false)]
            [TestCase("\"a\">\"a\"", false)]
            [TestCase("\"a\"!=\"a\"", false)]
            [TestCase("\"a\">=\"a\"", true)]
            [TestCase("\"a\"<=\"a\"", true)]
            [TestCase("\"one\"==\"two\"", false)]
            [TestCase("12.34 == 23.45", false)]
            [TestCase("12.34 <  23.45", true)]
            [TestCase("12.34 >  23.45", false)]
            [TestCase("12.34 != 23.45", true)]
            [TestCase("12.34 >= 23.45", false)]
            [TestCase("12.34 <= 23.45", true)]
            [TestCase("52.34 == 23.45", false)]
            [TestCase("52.34 <  23.45", false)]
            [TestCase("52.34 >  23.45", true)]
            [TestCase("52.34 != 23.45", true)]
            [TestCase("52.34 >= 23.45", true)]
            [TestCase("52.34 <= 23.45", false)]
            [TestCase("12.34 == 12.34", true)]
            [TestCase("12.34 <  12.34", false)]
            [TestCase("12.34 >  12.34", false)]
            [TestCase("12.34 != 12.34", false)]
            [TestCase("12.34 >= 12.34", true)]
            [TestCase("12.34 <= 12.34", true)]
            [TestCase("4 == 4", true)]
            [TestCase("4 <  4", false)]
            [TestCase("4 >  4", false)]
            [TestCase("4 != 4", false)]
            [TestCase("4 >= 4", true)]
            [TestCase("4 <= 4", true)]
            [TestCase("12.0 + 12.5", 24.5)]
            [TestCase("12.0 * 12.5", 150.0)]
            [TestCase("12.3 - 12.3", 0.0)]
            [TestCase("12.3 / 12.3", 1.0)]
            [TestCase("12.3 ^ 12.3", 25458520501262.152)]
            [TestCase("4 + 4", 8)]
            [TestCase("4 * 4", 16)]
            [TestCase("4 - 4", 0)]
            [TestCase("4 / 4", 1)]
            [TestCase("4 ^ 4", 256)]
            [TestCase("\"a\" + \"a\"", "aa")]
            [TestCase("\"a\" - \"a\"", null, ExpectedException = typeof(InvalidOperationException))]
            [TestCase("\"a\" * \"a\"", null, ExpectedException = typeof(InvalidOperationException))]
            [TestCase("\"a\" / \"a\"", null, ExpectedException = typeof(InvalidOperationException))]
            [TestCase("\"a\" ^ \"a\"", null, ExpectedException = typeof(InvalidOperationException))]
            [TestCase("\"a\" && \"a\"", null, ExpectedException = typeof(InvalidOperationException))]
            [TestCase("\"a\" || \"a\"", null, ExpectedException = typeof(InvalidOperationException))]
            [TestCase("!\"a\"", null, ExpectedException = typeof(InvalidOperationException))]
            [TestCase("\"a\"?1:2", null, ExpectedException = typeof(InvalidCastException))]
            public void EvaluateExpression(string source, object expected)
            {
                var expression = CreateExpression(source);

                var evaluator = new EvaluateVisitor();
                var scope = new Scope();

                var result = expression.Accept(evaluator, scope);

                Assert.AreEqual(expected, result.ToObject());
            }
        }
    }
}
