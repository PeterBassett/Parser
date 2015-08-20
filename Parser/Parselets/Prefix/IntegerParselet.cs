using System.Linq.Expressions;
using AST;
using Lexer;

namespace Parser.Parselets.Prefix
{
    internal class IntegerParselet : NumberParselet
    {
        protected override object Parse(string tokenValue)
        {
            return int.Parse(tokenValue);
        }
    }
}
