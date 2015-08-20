using System.Linq.Expressions;
using AST;
using Lexer;

namespace Parser.Parselets.Prefix
{
    internal class FloatParselet : NumberParselet
    {
        protected override object Parse(string tokenValue)
        {
            return float.Parse(tokenValue);
        }
    }
}
