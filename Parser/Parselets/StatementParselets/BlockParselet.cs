using System.Collections.Generic;
using AST.Statements;

namespace Parser.Parselets.StatementParselets
{
    public class BlockParselet : StatementParselet
    {
        private readonly string _endToken;

        public BlockParselet(string endToken)
        {
            _endToken = endToken;
        }

        public override IStatement Parse(Parser parser, Lexer.Token current)
        {
            var statements = new List<IStatement>();

            do
            {
                var expression = ParseStatement(parser);

                if (expression != null)
                    statements.Add(expression);

            } while (parser.Current.Type != _endToken);

            return new ScopeBlockStmt(statements);            
        }

        public override bool NeedsTerminator
        {
            get { return true; }
        }

        public override string Terminator
        {
            get { return _endToken; }
        }
    }
}
