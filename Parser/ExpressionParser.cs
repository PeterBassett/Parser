using System;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using AST.Expressions.Comparison;
using AST.Expressions.Logical;
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
            RegisterParselet("BOOLEAN", new BooleanParselet());
            RegisterParselet("LEFTPAREN", new GroupParselet());
            RegisterParselet("QUESTIONMARK", new ConditionalParselet());
            RegisterParselet("QUOTED-STRING", new StringParselet());

            InfixLeft<PlusExpr>("PLUS", Precedence.Sum);
            InfixLeft<MinusExpr>("MINUS", Precedence.Sum);
            InfixLeft<MultExpr>("MULT", Precedence.Product);
            InfixLeft<DivExpr>("DIV", Precedence.Product);

            InfixLeft<EqualsExpr>("EQUALS", Precedence.Equality);
            InfixLeft<NotEqualsExpr>("NOTEQUALS", Precedence.Equality);                
            InfixLeft<LessThanExpr>("LT", Precedence.Comparison);
            InfixLeft<LessThanOrEqualsExpr>("LTE", Precedence.Comparison);
            InfixLeft<GreaterThanExpr>("GT", Precedence.Comparison);
            InfixLeft<GreaterThanOrEqualsExpr>("GTE", Precedence.Comparison);

            InfixRight<OrExpr>("BOOLEAN-OR", Precedence.Logical);
            InfixRight<AndExpr>("BOOLEAN-AND", Precedence.Logical);

            Prefix<NotExpr>("BOOLEAN-NOT", Precedence.Unary);
            Prefix<NegationExpr>("MINUS", Precedence.Unary);

            InfixRight<PowExpr>("POW", Precedence.Exponent);
        }

        private void InfixLeft<T>(string token, Precedence precedence)
        {
            RegisterBinaryOperatorParselet<T>(token, precedence, false);
        }

        public void InfixRight<T>(string token, Precedence precedence)
        {
            RegisterBinaryOperatorParselet<T>(token, precedence, true);
        }

        private void RegisterBinaryOperatorParselet<T>(string token, Precedence precedence, bool isRightAssociative)
        {
            var constructedType = typeof (BinaryOperatorParselet<>).MakeGenericType(new[] {typeof (T)});

            RegisterParselet(token, (IInfixParselet)Activator.CreateInstance(constructedType, new object[] { precedence, isRightAssociative }));
        }

        public void Prefix<T>(string token, Precedence precedence)
        {
            var constructedType = typeof(UnaryOperatorParselet<>).MakeGenericType(new[] { typeof(T) });

            RegisterParselet(token, (IPrefixParselet)Activator.CreateInstance(constructedType, new object[] { precedence }));
        }
    }
}
