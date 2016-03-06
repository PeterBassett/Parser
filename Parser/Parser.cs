using System;
using System.Collections.Generic;
using System.Linq;
using AST;
using AST.Statements;
using Lexer;
using Parser.Parselets.Infix;
using Parser.Parselets.Prefix;
using Parser.Parselets.StatementParselets;

namespace Parser
{
    public abstract class Parser : IParser
    {
        private readonly ILexer _lexer;
        private readonly List<Token> _tokenQueue;
        private readonly Dictionary<string, IPrefixParselet> _prefixParselets;
        private readonly Dictionary<string, IInfixParselet> _infixParselets;
        private readonly Dictionary<string, StatementParselet> _statementParselets;

        protected Parser(ILexer lexer)
        {
            if (lexer == null)
                throw new ArgumentNullException("lexer");
            _lexer = lexer;

            _tokenQueue = new List<Token>();
            _prefixParselets = new Dictionary<string, IPrefixParselet>();
            _infixParselets = new Dictionary<string, IInfixParselet>();
            _statementParselets = new Dictionary<string, StatementParselet>();
        }

        public Expression ParseAll()
        {
            return ParseStatementList(0);
        }

        private Expression ParseStatementList(int precedence)
        {
            var statementList = new List<Statement>();

            do
            {
                var stmt = ParseNext();

                if (!(stmt is Statement))
                    return stmt;

                statementList.Add(stmt as Statement);

            } while (Peek().Type != "EMPTY");

            if(statementList.Count> 1)
                return new StatementList(statementList);

            return statementList.First();
        }

        public Expression ParseNext()
        {
            var token = Consume();

            var statement = GetStatementParserForCurrentToken(token);

            if (statement == null)
                return ParseExpression(0, token);

            var stmt = statement.Parse(this, token);

            if (statement.NeedsTerminator)
                Consume(statement.Terminator);

            return stmt;            
        }

        public Expression ParseExpression(int precedence)
        {
            var token = Consume();

            return ParseExpression(precedence, token);
        }

        private Expression ParseExpression(int precedence, Token token)
        {
            var prefix = GetPrefixParserForCurrentToken(token);

            if (prefix == null)
                throw new ParseException("Could not parse \"" + _lexer.Current + "\".");

            var left = prefix.Parse(this, token);

            while (precedence < GetPrecedence())
            {
                token = Consume();

                var infix = GetInfixParserForCurrentToken(token);
                left = infix.Parse(this, left, token);
            }

            return left;
        }

        public Token Current
        {
            get { return LookAhead(); }
        }

        protected void RegisterParselet(string tokenType, IPrefixParselet parselet)
        {
            _prefixParselets.Add(tokenType, parselet);
        }

        protected void RegisterParselet(string tokenType, IInfixParselet parselet)
        {
            _infixParselets.Add(tokenType, parselet);
        }

        protected void RegisterParselet(string tokenType, StatementParselet parselet)
        {
            _statementParselets.Add(tokenType, parselet);
        }

        private IPrefixParselet GetPrefixParserForCurrentToken(Token token)
        {
            var type = token.Type;

            if (_prefixParselets.ContainsKey(type))
                return _prefixParselets[type];

            return null;
        }

        private IInfixParselet GetInfixParserForCurrentToken(Token token)
        {
            var type = token.Type;

            if (_infixParselets.ContainsKey(type))
                return _infixParselets[type];

            return null;
        }

        private StatementParselet GetStatementParserForCurrentToken(Token token)
        {
            var type = token.Type;

            if (_statementParselets.ContainsKey(type))
                return _statementParselets[type];

            return null;
        }

        private Token Consume()
        {
            // Make sure we've read the token.
            var token = LookAhead();

            _tokenQueue.RemoveAt(0);

            return token;
        }

        public void Consume(string expected)
        {
            var token = LookAhead();
            if (token.Type != expected)
                throw new ParseException("Expected token " + expected + " and found " + token.Type + " at " + token);

            Consume();
        }

        public bool ConsumeOptional(string expected)
        {
            var token = LookAhead();

            if (token.Type == expected)
            {
                Consume();
                return true;
            }

            return false;
        }

        private int GetPrecedence()
        {
            var type = LookAhead().Type;

            if (_infixParselets.ContainsKey(type))
            {
                var parser = _infixParselets[type];
                if (parser != null)
                    return parser.Precedence;
            }

            return 0;
        }

        private Token LookAhead(int distance = 0)
        {
            // Read in as many as needed.
            while (distance >= _tokenQueue.Count)
            {
                _lexer.Advance();
                _tokenQueue.Add(_lexer.Current);
            }

            // Get the queued token.
            return _tokenQueue[distance];
        }

        internal Token Peek()
        {
            LookAhead();
            return _tokenQueue[0];
        }
    }
}
