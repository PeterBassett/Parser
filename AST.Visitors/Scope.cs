using System;
using System.Collections.Generic;
using AST.Statements;
using AST.Expressions.Arithmatic;
using AST.Expressions.Function;

namespace AST.Visitor
{
    public class Scope : IDisposable
    {
        private readonly Scope _parent;
        private readonly Dictionary<string, Identifier> _values;

        public Scope()
            : this(null)
        {
        }

        public Scope(Scope parent)
        {
            _parent = parent;
            _values = new Dictionary<string, Identifier>();
        }

        public struct Identifier
        {
            public static Identifier Undefined = new Identifier(false);            
            private readonly bool _isDefined;
            private Value _value;

            private Identifier(bool unused)
                : this()
            {
                _isDefined = false;
                _value = null;
            }

            public Identifier(Value value)
            {
                _value = value;
                _isDefined = true;
            }

            public Value Value { get { return _value; } }
            public bool IsDefined { get { return _isDefined; } }
        }

        private Scope Parent { get { return _parent; } }

        public void DefineIdentifier(string name, Value value)
        {
            _values.Add(name, new Identifier(value));
        }

        public Identifier FindIdentifier(string name)
        {
            if (_values.ContainsKey(name))
                return _values[name];

            if (Parent != null)
            {
                return Parent.FindIdentifier(name);
            }

            return Identifier.Undefined;
        }

        public void Dispose()
        {
            PopScope();
        }

        public Scope PushArguments(VarDefinitionStmt[] arguments, Value[] values)
        {
            var newScope = PushScope();

            for (int i = 0; i < arguments.Length; i++)
            {
                newScope.DefineIdentifier(arguments[i].Name.Name, values[i]);
            }

            return newScope;
        }

        public Scope PushScope()
        {
            return new Scope(this);
        }

        public Scope PopScope()
        {
            return _parent;
        }
    }
}
