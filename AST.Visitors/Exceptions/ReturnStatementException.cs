﻿using System;

namespace AST.Visitor.Exceptions
{
    public class ReturnStatementException : Exception
    {
        private readonly Value _value;
        public ReturnStatementException(Value value)
        {
            _value = value;
        }

        public Value Value { get { return _value; } }
    }
}
