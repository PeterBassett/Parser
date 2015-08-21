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
        public class TestClass
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
            [TestCase("!false", true)]
            [TestCase("!true", false)]
            [TestCase("true||true&&false", true)]
            [TestCase("true || true && false", true)]            
            [TestCase("(true||true)&&false", false)]
            [TestCase("true||(true&&false)", true)]
            [TestCase("1*2*3*4*5*6", 720)]
            [TestCase("6*5*4*3*2*1", 720)]
            [TestCase("7^5", 16807)]
            [TestCase("7^5/5", 3361.4)]
            [TestCase("true?1:2", 1)]
            [TestCase("false?1:2", 2)]
            [TestCase("1+2==3?\"TRUE!\":\"FALSE!\"", "TRUE!")]
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
