using System;

namespace AST.Visitor
{
    public class ReturnStatementExpectedException : Exception
    {
        public ReturnStatementExpectedException(string message) : base(message)
        {
            
        }
    }
}
