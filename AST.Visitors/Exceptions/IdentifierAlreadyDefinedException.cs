using System;

namespace AST.Visitor.Exceptions
{
    public class IdentifierAlreadyDefinedException : Exception
    {
        public IdentifierAlreadyDefinedException(string identifier)
            : base(string.Format("An identifier '{0}' is already defined in this scope", identifier))
        {
        }
    }
}
