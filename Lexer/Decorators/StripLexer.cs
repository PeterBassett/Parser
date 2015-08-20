using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer.Decorators
{
    class StripLexer : ILexer
    {
        private readonly ILexer _lexer;
        public StripLexer(ILexer lexer)
        {
            if(lexer == null)
                throw new ArgumentNullException("lexer");
            _lexer = lexer;
        }

        public bool Advance()
        {
            while (_lexer.Advance())
            {
                switch (_lexer.Current.Type)
                {
                    case "WHITESPACE":
                    case "COMMENT":
                        break;
                    default:
                        return true;
                }
            }

            return false;
        }

        public Token Current
        {
            get { return _lexer.Current; }
        }
    }
}
