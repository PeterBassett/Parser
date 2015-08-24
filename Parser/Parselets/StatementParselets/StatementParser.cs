using System.Collections.Generic;
using AST;
using AST.Expressions;
using AST.Statements;
using Parser.Parselets.StatementParselets;

namespace Parser.Parselets.Prefix.Statements
{
    public abstract class StatementParser : IStatementParselet
    {
        protected BlockStmt Block(Parser parser, Lexer.Token current, string beginToken, string endToken)
        {
            var statements = new List<IStatement>();

            parser.Consume(beginToken);
            do
            {
                var expression = parser.Parse();

                if(!(expression is IStatement))
                    throw new ParseException("Invalid statement in block.");

                statements.Add(expression as IStatement);

                parser.Consume("SEMICOLON");
            } while (parser.Current.Type != endToken);

            return new BlockStmt(statements);
        }

        public abstract IStatement Parse(Parser parser, Lexer.Token current);
        /*
        public IStatement Parse(Parser parser, Lexer.Token current)
        {
        // Statements
            var a = [], s;

            while (true) {
                if (token.Lexeme == "}" || token.Lexeme == "") 
                {
                    yield break;
                }

                s = statement();
                if (s) {
                    a.push(s);
                }
            }

            return a.length === 0 ? null : a.length === 1 ? a[0] : a;
        }

        public IEnumerable<IExpression> Statement(Parser parser, Lexer.Token token)
        {        
            var n = token, v;
            if (n.std) {
                advance();
                scope.reserve(n);
                return n.std();
            }
            v = expression(0);
            if (!v.assignment && v.id !== "(") {
                v.error("Bad expression statement.");
            }
            advance(";");
            return v;
        }*/


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
