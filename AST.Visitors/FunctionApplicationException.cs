using System;

namespace AST.Visitor
{
    class FunctionApplicationException : Exception
    {
        public FunctionApplicationException(string message)
            : base(message)
        {
            
        }
    }
}
