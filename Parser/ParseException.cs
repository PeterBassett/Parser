using System;

namespace Parser
{
    internal class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        {
        }
    }
}
