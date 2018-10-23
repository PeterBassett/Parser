using System.Collections.Generic;
using AST.Statements;

namespace Parser.Parselets.StatementParselets
{
    public class SemiColonParselet : StatementParselet
    {
        public override Statement Parse(Parser parser, Lexer.Token current)
        {            
            return new NullStatement();            
        }
    }
}
