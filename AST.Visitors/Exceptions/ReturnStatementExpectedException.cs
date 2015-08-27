using System;

namespace AST.Visitor.Exceptions
{
    public class ReturnStatementExpectedException : Exception
    {
        public ReturnStatementExpectedException(string message) : base(message)
        {
            
        }
    }
}
