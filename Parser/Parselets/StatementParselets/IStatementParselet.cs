using AST;
using AST.Statements;
using Lexer;

namespace Parser.Parselets.StatementParselets
{
    public interface IStatementParselet
    {
        IStatement Parse(Parser parser, Token current);
        bool NeedsTerminator { get; }
        string Terminator { get;}
    }
}
