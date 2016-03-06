﻿using System;
using AST;
using AST.Expressions;
using Lexer;

namespace Parser.Parselets.Prefix
{
    public class UnaryOperatorParselet<T> : IPrefixParselet where T : UnaryOperatorExpr
    {
        private readonly int _precedence;
        
        public UnaryOperatorParselet(int precedence)
        {
            _precedence = precedence;
        }

        public Expression Parse(Parser parser, Token token)
        {           
            var right = parser.ParseExpression(_precedence);

            return (Expression)Activator.CreateInstance(typeof(T), new object[] { right });
        }
    }
}
