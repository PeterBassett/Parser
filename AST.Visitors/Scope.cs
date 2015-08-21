using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AST.Visitor
{
    public class Scope
    {
        public struct Identifier
        {
            public bool IsDefined { get; set; }

            internal Value GetCurrentValue()
            {
                return new Value(0);
            }
        }

        internal Identifier FindIdentifier(string identifierName)
        {
            return new Identifier();
        }
    }
}
