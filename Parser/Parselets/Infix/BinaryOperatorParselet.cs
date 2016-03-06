﻿using System;
using AST;
using AST.Expressions;
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

        public Expression Parse(Parser parser, Expression left, Token token)
        {           
            var right = parser.ParseExpression(_precedence - (_isRightAssociative ? 1 : 0));

            return (Expression)Activator.CreateInstance(typeof(T), new object[] { left, right });
        }

        public int Precedence
        {
            get { return _precedence; }
        }
    }
}
