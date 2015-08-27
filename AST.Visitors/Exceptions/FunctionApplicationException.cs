using System;

namespace AST.Visitor.Exceptions
{
    class FunctionApplicationException : Exception
    {
        public FunctionApplicationException(string message)
            : base(message)
        {
            
        }
    }
}
