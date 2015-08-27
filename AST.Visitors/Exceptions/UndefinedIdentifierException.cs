using System;

namespace AST.Visitor.Exceptions
{
    public class UndefinedIdentifierException : Exception
    {
        public UndefinedIdentifierException(string message)
            : base(message)
        {                
        }
    }
}
