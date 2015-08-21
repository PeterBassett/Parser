using System;

namespace AST.Visitor
{
    public class TypeCheckException : Exception
    {
        public TypeCheckException(string message)
            : base(message)
        {
        }
    }
}
