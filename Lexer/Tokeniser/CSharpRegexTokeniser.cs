using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Lexer.Tokeniser
{
    public class CSharpRegexTokeniser : IRegexTokeniser
    {
        private static readonly List<RegexTokeniser> TokeniserList;

        static CSharpRegexTokeniser()
        {
            TokeniserList = new List<RegexTokeniser>
            {
                new RegexTokeniser(@"\d+", "INTEGER"),
                new RegexTokeniser(@"\d*\.\d+([eE][-+]?\d+)?", "FLOAT"),
                new RegexTokeniser(@"true|false", "BOOLEAN"),

                new RegexTokeniser(new Regex("/\\*(.*?)\\*/", RegexOptions.Singleline), "COMMENT"),    
                
                new RegexTokeniser(@"([""])(?:\\\1|.)*?\1", "QUOTED-STRING"),
                new RegexTokeniser(@"'[A-Za-z0-9]'", "QUOTED-CHAR"),
                
                new RegexTokeniser(@"if", "IF"),
                new RegexTokeniser(@"else", "ELSE"),

                new RegexTokeniser(@"var", "VAR"),
                new RegexTokeniser(@"val", "VAL"),
                new RegexTokeniser(@"return", "RETURN"),
                new RegexTokeniser(@"function", "FUNCTION"),

                new RegexTokeniser(@"\s+", "WHITESPACE"),
                new RegexTokeniser(@"[A-Za-z_]+[A-Za-z0-9_]*", "IDENTIFIER"),
                new RegexTokeniser(@"\.", "DOT"),
                new RegexTokeniser(@"\(", "LEFTPAREN"),
                new RegexTokeniser(@"\)", "RIGHTPAREN"),
                new RegexTokeniser(@"{", "LEFTBRACE"),
                new RegexTokeniser(@"}", "RIGHTBRACE"),
                new RegexTokeniser(@"=>", "RIGHTARROW"),
                new RegexTokeniser(@"\+", "PLUS"),
                new RegexTokeniser(@"-", "MINUS"),
                new RegexTokeniser(@"\*", "MULT"),
                new RegexTokeniser(@"/", "DIV"),
                new RegexTokeniser(@"%", "MOD"),
                new RegexTokeniser(@"\^", "POW"),
                new RegexTokeniser(@":", "COLON"),
                new RegexTokeniser(@";", "SEMICOLON"),
                new RegexTokeniser(@"=", "ASSIGNMENT"),
                new RegexTokeniser(@"==", "EQUALS"),
                new RegexTokeniser(@"!=", "NOTEQUALS"),
                new RegexTokeniser(@">", "GT"),
                new RegexTokeniser(@">=", "GTE"),
                new RegexTokeniser(@"<", "LT"),
                new RegexTokeniser(@"<=", "LTE"),
                new RegexTokeniser(@"\?", "QUESTIONMARK"),
                new RegexTokeniser(@"\[", "LEFTSQUAREBRACE"),
                new RegexTokeniser(@"\]", "RIGHTSQUAREBRACE"),
                new RegexTokeniser(@"\,", "COMMA"),
                new RegexTokeniser(@"\|", "BITWISE-OR"),
                new RegexTokeniser(@"\|\|", "BOOLEAN-OR"),
                new RegexTokeniser(@"\&", "BITWISE-AND"),
                new RegexTokeniser(@"\&\&", "BOOLEAN-AND"),
                new RegexTokeniser(@"~", "TILDE"),
                new RegexTokeniser(@"!", "BOOLEAN-NOT")
            };
        }

        public IEnumerable<RegexTokeniser> Tokenisers
        {
            get { return TokeniserList; }
        }
    }
}
