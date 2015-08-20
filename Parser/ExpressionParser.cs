using System;
using AST.Expressions.Arithmatic;
using Lexer;
using Parser.Parselets.Infix;
using Parser.Parselets.Prefix;

namespace Parser
{
    public class ExpressionParser : Parser
    {
        public ExpressionParser(ILexer lexer) : base(lexer)
        {
            RegisterParselet("INTEGER", new IntegerParselet());
            RegisterParselet("FLOAT", new FloatParselet());
            RegisterParselet("LEFTPAREN", new GroupParselet());
            InfixLeft<PlusExpr>("PLUS", Precedence.Sum);
            InfixLeft<MinusExpr>("MINUS", Precedence.Sum);
            InfixLeft<MultExpr>("MULT", Precedence.Product);
            InfixLeft<DivExpr>("DIV", Precedence.Product);
        }

        private void InfixLeft<T>(string token, Precedence precedence)
        {
            var constructedType = typeof(BinaryOperatorParselet<>).MakeGenericType(new[] { typeof(T) });

            RegisterParselet(token, (IInfixParselet)Activator.CreateInstance(constructedType, new object[] { precedence, false }));
        }
    }
}
