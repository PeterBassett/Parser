using System;
using System.Collections.Generic;
using System.Linq;
using Lexer.Decorators;
using Lexer.Tokeniser;
using Moq;
using NUnit.Framework;

namespace Lexer.Tests.Decorators
{
    namespace StripLexerTests
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void TakesAnILexer()
            {
                new StripLexer(new Mock<ILexer>().Object);
            }

            [TestCase(ExpectedException = typeof(ArgumentNullException))]
            public void ThrowsOnNull()
            {
                new StripLexer(null);
            }
        }

        [TestFixture]
        public class TokenGenerationTests
        {
            [TestCase(new[] { "INTEGER" },
                    new[] { "1" })]
            [TestCase(new[] { "INTEGER" },
                    new[] { "1234" })]
            [TestCase(new[] { "FLOAT" },
                    new[] { "12.34" })]            
            [TestCase(new[] { "WHITESPACE" },
                    new[] { " \t\r\n \t" })]            
            [TestCase(new[] {"INTEGER", "WHITESPACE", "INTEGER", "WHITESPACE", "FLOAT"},
                    new[] {"1", " ", "123", " ", "12.34"})]
            [TestCase(new[] { "INTEGER", "WHITESPACE", "INTEGER", "WHITESPACE", "FLOAT", "WHITESPACE", "INTEGER" },
                    new[] { "1", " ", "123", "  \r\n  ", "12.34", "\t\t", "1234567890" })]
            [TestCase(new[] { "QUOTED-STRING" },
                    new[] { "\"THIS IS A TEST\"" })]
            [TestCase(new[] { "INTEGER", "WHITESPACE", "QUOTED-STRING" },
                    new[] { "1", " ", "\"THIS IS A TEST\"" })]
            [TestCase(new[] { "QUOTED-CHAR" },
                    new[] { "'a'" })]
            [TestCase(new[] { "COMMENT" },
                    new[] { "/* comment here */" })]
            [TestCase(new[] { "INTEGER", "WHITESPACE", "COMMENT", "WHITESPACE" },
                    new[] { "1", " \t \t ", "/* 123 \r\n \"THIS IS A \r\nTEST\" \r\n */", " "})]
            [TestCase(new[] { "IDENTIFIER" },
                    new[] { "a" })]
            [TestCase(new[] { "IDENTIFIER" },
                    new[] { "abc" })]
            [TestCase(new[] { "IDENTIFIER" },
                    new[] { "a1b2c" })]
            [TestCase(new[] { "IDENTIFIER" },
                    new[] { "A" })]
            [TestCase(new[] { "IDENTIFIER" },
                    new[] { "A1" })]
            [TestCase(new[] { "IDENTIFIER" },
                    new[] { "_" })]
            [TestCase(new[] { "IDENTIFIER" },
                    new[] { "_a" })]
            [TestCase(new[] { "DOT" },
                    new[] { "." })] 
            [TestCase(new[] { "IDENTIFIER", "DOT", "IDENTIFIER", "DOT", "IDENTIFIER", "DOT", "IDENTIFIER", "DOT", "IDENTIFIER", "DOT", "IDENTIFIER"},
                    new[] { "this", ".", "is", ".", "a", ".", "dot", ".", "notation", ".", "test" })]
            [TestCase(new[] { "LEFTPAREN" },
                    new[] { "(" })]
            [TestCase(new[] { "RIGHTPAREN" },
                    new[] { ")" })]
            [TestCase(new[] { "LEFTPAREN", "INTEGER", "WHITESPACE", "IDENTIFIER", "RIGHTPAREN" },
                    new[] { "(", "1234", " ", "abc", ")" })]
            [TestCase(new[] { "LEFTBRACE" },
                    new[] { "{" })] 
            [TestCase(new[] { "RIGHTBRACE" },
                    new[] { "}" })]
            [TestCase(new[] { "LEFTBRACE", "INTEGER", "WHITESPACE", "IDENTIFIER", "RIGHTBRACE" },
                    new[] { "{", "1234", " ", "abc", "}" })]
            [TestCase(new[] { "LEFTBRACE", "RIGHTPAREN", "RIGHTBRACE", "LEFTPAREN" },
                    new[] { "{", ")", "}", "(" })]
            [TestCase(new[] { "LEFTBRACE", "RIGHTPAREN", "RIGHTBRACE", "LEFTPAREN" },
                    new[] { "{", ")", "}", "(" })]
            [TestCase(new[] { "LEFTSQUAREBRACE" },
                    new[] { "[" })]
            [TestCase(new[] { "RIGHTSQUAREBRACE" },
                    new[] { "]" })]
            [TestCase(new[] { "RIGHTSQUAREBRACE", "LEFTBRACE", "LEFTSQUAREBRACE", "RIGHTPAREN", "RIGHTSQUAREBRACE", "RIGHTBRACE", "LEFTPAREN" },
                    new[] { "]", "{", "[", ")", "]", "}", "(" })]
            [TestCase(new[] { "MINUS" },
                    new[] { "-" })]
            [TestCase(new[] { "PLUS" },
                    new[] { "+" })]
            [TestCase(new[] { "MULT" },
                    new[] { "*" })]
            [TestCase(new[] { "DIV" },
                    new[] { "/" })]
            [TestCase(new[] { "MOD" },
                    new[] { "%" })]
            [TestCase(new[] { "BOOLEAN-NOT" },
                    new[] { "!" })]
            [TestCase(new[] { "ASSIGNMENT" },
                    new[] { "=" })]
            [TestCase(new[] { "EQUALS" },
                    new[] { "==" })]
            [TestCase(new[] { "NOTEQUALS" },
                    new[] { "!=" })]
            [TestCase(new[] { "GT" },
                    new[] { ">" })]
            [TestCase(new[] { "LT" },
                    new[] { "<" })]
            [TestCase(new[] { "GTE" },
                    new[] { ">=" })]
            [TestCase(new[] { "LTE" },
                    new[] { "<=" })]
            [TestCase(new[] { "COMMA" },
                    new[] { "," })]
            [TestCase(new[] { "SEMICOLON" },
                    new[] { ";" })]
            [TestCase(new[] { "QUESTIONMARK" },
                    new[] { "?" })]
            [TestCase(new[] { "COLON" },
                    new[] { ":" })]
            [TestCase(new[] { "TILDE" },
                    new[] { "~" })]
            [TestCase(new[] { "BITWISE-OR" },
                    new[] { "|" })]
            [TestCase(new[] { "BOOLEAN-OR" },
                    new[] { "||" })]
            [TestCase(new[] { "BITWISE-AND" },
                    new[] { "&" })]
            [TestCase(new[] { "BOOLEAN-AND" },
                    new[] { "&&" })]
            [TestCase(new[] { "POW" },
                    new[] { "^" })]
            [TestCase(new[] { "INTEGER", "POW", "INTEGER"},
                    new[] { "7","^","5" })]
            [TestCase(new[]
                    {
                        "LEFTPAREN", "INTEGER", "PLUS", "INTEGER", "MULT", "INTEGER", "DIV", "INTEGER", "RIGHTPAREN", "WHITESPACE", "EQUALS", "WHITESPACE", "FLOAT", "WHITESPACE", "BOOLEAN-AND", "WHITESPACE",
                        "IDENTIFIER", "DOT", "IDENTIFIER", "LEFTPAREN", "RIGHTPAREN", "WHITESPACE", "NOTEQUALS", "WHITESPACE", "QUOTED-STRING"
                    },
                    new[] { "(", "1", "+", "2", "*", "7", "/", "4", ")", " ", "==", " ", "4.5", " ", "&&", " ", "this", ".", "GetDay", "(", ")", " ", "!=", " ", "\"Tuesday\"" })]
            [TestCase(new[]
                    {
                        "IDENTIFIER", "WHITESPACE", "LTE", "WHITESPACE", "IDENTIFIER", "WHITESPACE", "BOOLEAN-OR", "WHITESPACE",
                        "INTEGER", "GT", "INTEGER"
                    },
                    new[] { "one", " ", "<=", " ", "two", " ", "||", " ", "4", ">", "5" })]
            [TestCase(new[]
                    {
                        "IDENTIFIER", "ASSIGNMENT", "IDENTIFIER", "EQUALS", "IDENTIFIER"                        
                    },
                    new[] { "one", "=", "two", "==", "three" })]
            [TestCase(new[] { "BOOLEAN" },
                    new[] { "true"})]
            [TestCase(new[] { "BOOLEAN" },
                    new[] { "false" })]

            [TestCase(new[] { "BOOLEAN", "WHITESPACE", "BOOLEAN-AND", "WHITESPACE", "BOOLEAN" },
                    new[] { "false", " ", "&&", " ", "true" })]
            public void TestTokenisation(string[] types, string[] lexemes)
            {
                var generatedTokens = GenerateTokens(types, lexemes);

                GeneratedTokenTypesContainNoWhiteSpaceOrComments(generatedTokens);
                ValidateGeneratedTypesAndLexemes(generatedTokens, types, lexemes);
            }

            private List<Token> GenerateTokens(string[] types, string[] lexemes)
            {
                var tokenIndex = -1;
                var lexer = new Mock<ILexer>();
                lexer.Setup(l => l.Advance()).Returns(() =>
                {
                    tokenIndex ++;
                    return (tokenIndex < types.Length);
                });

                lexer.Setup(l => l.Current).Returns(() =>
                {
                    if (tokenIndex < 0 || tokenIndex > types.Length - 1)
                        return Token.Empty;

                    return new Token(types[tokenIndex], lexemes[tokenIndex], 0, 0, 0);
                });

                return Tokenise(new StripLexer(lexer.Object));
            }

            private static List<Token> Tokenise(ILexer target)
            {
                var tokens = new List<Token>();

                while (target.Advance())
                    tokens.Add(target.Current);

                return tokens;
            }

            private void GeneratedTokenTypesContainNoWhiteSpaceOrComments(List<Token> generatedTokens)
            {
                Assert.IsFalse( generatedTokens.Any(t => t.Type == "WHITESPACE" || t.Type == "COMMENT") );
            }

            private void ValidateGeneratedTypesAndLexemes(List<Token> generatedTokens, string[] expectedtypes, string[] expectedLexemes)
            {
                var culledTypes = expectedtypes.Zip(expectedLexemes, (s, s1) => new {Type = s, Lexeme = s1}).
                    Where(t => t.Type != "WHITESPACE" && t.Type != "COMMENT").ToArray();

                Assert.AreEqual(generatedTokens.Count, culledTypes.Length);

                for (int i = 0; i < generatedTokens.Count; i++)
                {
                    Assert.AreEqual(generatedTokens[i].Type, culledTypes[i].Type);
                    Assert.AreEqual(generatedTokens[i].Lexeme, culledTypes[i].Lexeme);
                }
            }
        }
    }
}
