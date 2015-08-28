using System;
using AST;
using AST.Visitor;
using AST.Visitor.Exceptions;
using Lexer;
using Lexer.Decorators;
using Lexer.Tokeniser;
using NUnit.Framework;

namespace Parser.Tests
{
    namespace FullStatementParserTests
    {
        [TestFixture]
        public class FullStatementParserTest
        {            
            public StatementParser SetupParser(string source)
            {
                return new StatementParser(new StripLexer(new Scanner(source, new CSharpRegexTokeniser())));
            }

            public IExpression CreateExpression(string source)
            {
                var target = SetupParser(source);

                return target.ParseAll();;
            }

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
                EvaluateTest(source, expected);
            }

            [TestCase("var a : int = 1+2; return a;", 3)]
            [TestCase("function Double (n int) => n * 2; val Test : int = Double (4); return Test;", 8)]
            [TestCase("function Square (n int) => n * n; val Test : int = Square (9); return Test;", 81)]            
            [TestCase(@"function fibonacci( n int )
            {
                if(n == 0)
                    return 0;
                else if(n == 1)
                    return 1;
                else
                    return fibonacci(n - 1) + fibonacci(n - 2);
            }

            return fibonacci(8);
            ", 21)]
            public void EvaluateStatement(string source, object expected)
            {
                EvaluateTest(source, expected);
            }

            public void EvaluateTest(string source, object expected)
            {
                var expression = CreateExpression(source);

                var evaluator = new EvaluateVisitor();
                var scope = new Scope();

                Value value = null;
                try
                {
                    value = expression.Accept(evaluator, scope);
                }
                catch (ReturnStatementException ret)
                { 
                    value = ret.Value;
                }

                Assert.AreEqual(expected, value.ToObject());
            }
        }
    }
}
