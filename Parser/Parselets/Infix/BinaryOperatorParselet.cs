using System;
using AST;
using AST.Expressions;
using AST.Expressions.Arithmatic;
using Lexer;

namespace Parser.Parselets.Infix
{
    public class BinaryOperatorParselet<T> : IInfixParselet where T : BinaryOperatorExpr
    {
        private readonly int _precedence;
        private readonly bool _isRightAssociative;

        public BinaryOperatorParselet(int precedence, bool isRightAssociative)
        {
            _precedence = precedence;
            _isRightAssociative = isRightAssociative;
        }

        public IExpression Parse(Parser parser, IExpression left, Token token)
        {           
            var right = parser.Parse(_precedence - (_isRightAssociative ? 1 : 0));

            return (IExpression)Activator.CreateInstance(typeof(T), new object[] { left, right });
        }

        public int Precedence
        {
            get { return _precedence; }
        }
    }
}
