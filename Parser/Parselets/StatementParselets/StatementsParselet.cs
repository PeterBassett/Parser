using AST.Statements;
using Lexer;

namespace Parser.Parselets.StatementParselets
{
    public abstract class StatementParselet : IStatementParselet
    {
        public abstract Statement Parse(Parser parser, Token current);

        protected Statement ParseStatement(Parser parser)
        {
            var expression = parser.ParseNext();

            if (expression == null) 
                return null;

            if (!(expression is Statement))
                throw new ParseException("Invalid statement in block.");

            if(!(expression is IBlockStatement))
                parser.ConsumeOptional("SEMICOLON");

            return expression as Statement;
        }

        public virtual bool NeedsTerminator
        {
            get { return false; }
        }

        public virtual string Terminator
        {
            get { return ""; }
        }        
    }
}
