using System;

namespace AST.Visitor
{
    public class UndefinedIdentifierException : Exception
    {
        public UndefinedIdentifierException(string message)
            : base(message)
        {                
        }
    }
}
