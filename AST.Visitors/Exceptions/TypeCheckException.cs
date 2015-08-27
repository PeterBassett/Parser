using System;

namespace AST.Visitor.Exceptions
{
    public class TypeCheckException : Exception
    {
        public TypeCheckException(string message)
            : base(message)
        {
        }
    }
}
