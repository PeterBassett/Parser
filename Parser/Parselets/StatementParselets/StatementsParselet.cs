using AST.Statements;
using Lexer;

namespace Parser.Parselets.StatementParselets
{
    public abstract class StatementParselet : IStatementParselet
    {
        public abstract IStatement Parse(Parser parser, Token current);

        protected IStatement ParseStatement(Parser parser)
        {
            var expression = parser.Parse();

            if (expression == null) 
                return null;

            if (!(expression is IStatement))
                throw new ParseException("Invalid statement in block.");

            if(!(expression is IBlockStatement))
                parser.Consume("SEMICOLON");

            return expression as IStatement;
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
