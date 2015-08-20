using System;
using System.Collections.Generic;
using System.Linq;
using Lexer.Tokeniser;
using Moq;
using NUnit.Framework;

namespace Lexer.Tests
{
    namespace ScannerTests
    {
        [TestFixture]
        public class Constructor
        {
            [TestCase]
            public void TakesTwoParameters()
            {
                new Scanner("source", new Mock<IRegexTokeniser>().Object);
            }

            [TestCase("a", true)]
            [TestCase("", true)]
            [TestCase(null, true, ExpectedException = typeof(ArgumentNullException))]
            [TestCase("a", false, ExpectedException = typeof(ArgumentNullException))]
            public void TakesSourceAndATokeniser(string source, bool validTokeniser)
            {
                IRegexTokeniser tokeniser = null;

                if (validTokeniser)
                    tokeniser = new Mock<IRegexTokeniser>().Object;

                new Scanner(source, tokeniser);
            }
        }

        [TestFixture]
        public class TokenGenerationTests
        {
            [TestCase("1",
                    new[] { "INTEGER" },
                    new[] { "1" })]
            [TestCase("1234",
                    new[] { "INTEGER" },
                    new[] { "1234" })]
            [TestCase("12.34",
                    new[] { "FLOAT" },
                    new[] { "12.34" })]            
            [TestCase(" \t\r\n \t",
                    new[] { "WHITESPACE" },
                    new[] { " \t\r\n \t" })]            
            [TestCase("1 123 12.34",
                    new[] {"INTEGER", "WHITESPACE", "INTEGER", "WHITESPACE", "FLOAT"},
                    new[] {"1", " ", "123", " ", "12.34"})]
            [TestCase("1 123  \r\n  12.34\t\t1234567890",
                    new[] { "INTEGER", "WHITESPACE", "INTEGER", "WHITESPACE", "FLOAT", "WHITESPACE", "INTEGER" },
                    new[] { "1", " ", "123", "  \r\n  ", "12.34", "\t\t", "1234567890" })]
            [TestCase("\"THIS IS A TEST\"",
                    new[] { "QUOTED-STRING" },
                    new[] { "\"THIS IS A TEST\"" })]
            [TestCase("1 \"THIS IS A TEST\"",
                    new[] { "INTEGER", "WHITESPACE", "QUOTED-STRING" },
                    new[] { "1", " ", "\"THIS IS A TEST\"" })]
            [TestCase("'a'",
                    new[] { "QUOTED-CHAR" },
                    new[] { "'a'" })]
            [TestCase("'ab'",
                    new string[] { },
                    new string[] { })]
            [TestCase("/* comment here */",
                    new[] { "COMMENT" },
                    new[] { "/* comment here */" })]
            [TestCase("1 \t \t /* 123 \r\n \"THIS IS A \r\nTEST\" \r\n */ ",
                    new[] { "INTEGER", "WHITESPACE", "COMMENT", "WHITESPACE" },
                    new[] { "1", " \t \t ", "/* 123 \r\n \"THIS IS A \r\nTEST\" \r\n */", " "})]
            [TestCase("a",
                    new[] { "IDENTIFIER" },
                    new[] { "a" })]
            [TestCase("abc",
                    new[] { "IDENTIFIER" },
                    new[] { "abc" })]
            [TestCase("a1b2c",
                    new[] { "IDENTIFIER" },
                    new[] { "a1b2c" })]
            [TestCase("A",
                    new[] { "IDENTIFIER" },
                    new[] { "A" })]
            [TestCase("A1",
                    new[] { "IDENTIFIER" },
                    new[] { "A1" })]
            [TestCase("_",
                    new[] { "IDENTIFIER" },
                    new[] { "_" })]
            [TestCase("_a",
                    new[] { "IDENTIFIER" },
                    new[] { "_a" })]
            [TestCase(".",
                    new[] { "DOT" },
                    new[] { "." })] 
            [TestCase("this.is.a.dot.notation.test",
                    new[] { "IDENTIFIER", "DOT", "IDENTIFIER", "DOT", "IDENTIFIER", "DOT", "IDENTIFIER", "DOT", "IDENTIFIER", "DOT", "IDENTIFIER"},
                    new[] { "this", ".", "is", ".", "a", ".", "dot", ".", "notation", ".", "test" })]
            [TestCase("(",
                    new[] { "LEFTPAREN" },
                    new[] { "(" })]
            [TestCase(")",
                    new[] { "RIGHTPAREN" },
                    new[] { ")" })]
            [TestCase("(1234 abc)",
                    new[] { "LEFTPAREN", "INTEGER", "WHITESPACE", "IDENTIFIER", "RIGHTPAREN" },
                    new[] { "(", "1234", " ", "abc", ")" })]
            [TestCase("{",
                    new[] { "LEFTBRACE" },
                    new[] { "{" })] 
            [TestCase("}",
                    new[] { "RIGHTBRACE" },
                    new[] { "}" })]
            [TestCase("{1234 abc}",
                    new[] { "LEFTBRACE", "INTEGER", "WHITESPACE", "IDENTIFIER", "RIGHTBRACE" },
                    new[] { "{", "1234", " ", "abc", "}" })]
            [TestCase("{)}(",
                    new[] { "LEFTBRACE", "RIGHTPAREN", "RIGHTBRACE", "LEFTPAREN" },
                    new[] { "{", ")", "}", "(" })]
            [TestCase("{)}(",
                    new[] { "LEFTBRACE", "RIGHTPAREN", "RIGHTBRACE", "LEFTPAREN" },
                    new[] { "{", ")", "}", "(" })]
            [TestCase("[",
                    new[] { "LEFTSQUAREBRACE" },
                    new[] { "[" })]
            [TestCase("]",
                    new[] { "RIGHTSQUAREBRACE" },
                    new[] { "]" })]
            [TestCase("]{[)]}(",
                    new[] { "RIGHTSQUAREBRACE", "LEFTBRACE", "LEFTSQUAREBRACE", "RIGHTPAREN", "RIGHTSQUAREBRACE", "RIGHTBRACE", "LEFTPAREN" },
                    new[] { "]", "{", "[", ")", "]", "}", "(" })]
            [TestCase("-",
                    new[] { "MINUS" },
                    new[] { "-" })]
            [TestCase("+",
                    new[] { "PLUS" },
                    new[] { "+" })]
            [TestCase("*",
                    new[] { "MULT" },
                    new[] { "*" })]
            [TestCase("/",
                    new[] { "DIV" },
                    new[] { "/" })]
            [TestCase("%",
                    new[] { "MOD" },
                    new[] { "%" })]
            [TestCase("!",
                    new[] { "NOT" },
                    new[] { "!" })]
            [TestCase("=",
                    new[] { "ASSIGNMENT" },
                    new[] { "=" })]
            [TestCase("==",
                    new[] { "EQUALS" },
                    new[] { "==" })]
            [TestCase("!=",
                    new[] { "NOTEQUALS" },
                    new[] { "!=" })]
            [TestCase(">",
                    new[] { "GT" },
                    new[] { ">" })]
            [TestCase("<",
                    new[] { "LT" },
                    new[] { "<" })]
            [TestCase(">=",
                    new[] { "GTE" },
                    new[] { ">=" })]
            [TestCase("<=",
                    new[] { "LTE" },
                    new[] { "<=" })]
            [TestCase(",",
                    new[] { "COMMA" },
                    new[] { "," })]
            [TestCase(";",
                    new[] { "SEMICOLON" },
                    new[] { ";" })]
            [TestCase("?",
                    new[] { "QUESTIONMARK" },
                    new[] { "?" })]
            [TestCase(":",
                    new[] { "COLON" },
                    new[] { ":" })]
            [TestCase("~",
                    new[] { "TILDE" },
                    new[] { "~" })]
            [TestCase("|",
                    new[] { "BITWISE-OR" },
                    new[] { "|" })]
            [TestCase("||",
                    new[] { "BOOLEAN-OR" },
                    new[] { "||" })]
            [TestCase("&",
                    new[] { "BITWISE-AND" },
                    new[] { "&" })]
            [TestCase("&&",
                    new[] { "BOOLEAN-AND" },
                    new[] { "&&" })]
            [TestCase("(1+2*7/4) == 4.5 && this.GetDay() != \"Tuesday\"",
                    new[]
                    {
                        "LEFTPAREN", "INTEGER", "PLUS", "INTEGER", "MULT", "INTEGER", "DIV", "INTEGER", "RIGHTPAREN", "WHITESPACE", "EQUALS", "WHITESPACE", "FLOAT", "WHITESPACE", "BOOLEAN-AND", "WHITESPACE",
                        "IDENTIFIER", "DOT", "IDENTIFIER", "LEFTPAREN", "RIGHTPAREN", "WHITESPACE", "NOTEQUALS", "WHITESPACE", "QUOTED-STRING"
                    },
                    new[] { "(", "1", "+", "2", "*", "7", "/", "4", ")", " ", "==", " ", "4.5", " ", "&&", " ", "this", ".", "GetDay", "(", ")", " ", "!=", " ", "\"Tuesday\"" })]
            [TestCase("one <= two || 4>5",
                    new[]
                    {
                        "IDENTIFIER", "WHITESPACE", "LTE", "WHITESPACE", "IDENTIFIER", "WHITESPACE", "BOOLEAN-OR", "WHITESPACE",
                        "INTEGER", "GT", "INTEGER"
                    },
                    new[] { "one", " ", "<=", " ", "two", " ", "||", " ", "4", ">", "5" })]
            [TestCase("one=two==three",
                    new[]
                    {
                        "IDENTIFIER", "ASSIGNMENT", "IDENTIFIER", "EQUALS", "IDENTIFIER"                        
                    },
                    new[] { "one", "=", "two", "==", "three" })]
            public void TestTokenisation(string source, string[] types, string[] lexemes)
            {
                var generatedTokens = GenerateTokens(source);

                ValidateGeneratedTokenTypes(generatedTokens, types);
                ValidateGeneratedLexemes(generatedTokens, lexemes);
            }

            private List<Token> GenerateTokens(string source)
            {
                return Tokenise(new Scanner(source, new CSharpRegexTokeniser()));
            }

            private static List<Token> Tokenise(ILexer target)
            {
                var tokens = new List<Token>();

                while (target.Advance())
                    tokens.Add(target.Current);

                return tokens;
            }

            private void ValidateGeneratedTokenTypes(List<Token> generatedTokens, string [] expectedTypes)
            {               
                Assert.AreEqual(expectedTypes.Length, generatedTokens.Count, string.Format("Count of found token types is incorrect. Found {0}", string.Join(", ", generatedTokens.Select(t => t.Type).ToArray())));

                for (int i = 0; i < expectedTypes.Length; i++)
                {
                    Assert.AreEqual(expectedTypes[i], generatedTokens[i].Type);
                }
            }

            private void ValidateGeneratedLexemes(List<Token> generatedTokens, string[] expectedLexemes)
            {
                Assert.AreEqual(expectedLexemes.Length, generatedTokens.Count, string.Format("Count of found lexemes is incorrect. Found {0}", string.Join(", ", generatedTokens.Select(t => t.Lexeme).ToArray())));

                for (int i = 0; i < expectedLexemes.Length; i++)
                {
                    Assert.AreEqual(expectedLexemes[i], generatedTokens[i].Lexeme);
                }
            }
        }
    }
}
