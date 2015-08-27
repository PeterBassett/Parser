using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AST.Statements;
using AST.Visitor.Exceptions;

namespace AST.Visitor
{
    public class Scope
    {
        private ScopeLevel _scope;

        public Scope()
        {
            _scope = new ScopeLevel();
        }

        public void DefineIdentifier(string name, Value value)
        {
            _scope.DefineIdentifier(name, value);
        }

        public Identifier FindIdentifier(string name)
        {
            return _scope.FindIdentifier(name);
        }

        public IDisposable PushArguments(VarDefinitionStmt[] arguments, Value[] values)
        {
            var scopePopper = PushScope();

            try
            {
                for (var i = 0; i < arguments.Length; i++)
                {
                    _scope.DefineIdentifier(arguments[i].Name.Name, values[i]);
                }
            }
            catch (Exception)
            {
                scopePopper.Dispose();
                throw;
            }

            return scopePopper;
        }

        public IDisposable PushScope()
        {
            _scope = new ScopeLevel(_scope);
            return new ScopePopper(this);
        }

        public void PopScope()
        {
            _scope = _scope.Parent;
        }

        private class ScopeLevel
        {
            private readonly ScopeLevel _parent;
            private readonly Dictionary<string, Identifier> _values;
            
            public ScopeLevel()
                : this(null)
            {
            }

            public ScopeLevel(ScopeLevel parent)
            {
                _values = new Dictionary<string, Identifier>();
                _parent = parent;
            }

            public ScopeLevel Parent
            {
                get { return _parent; }
            }

            public void DefineIdentifier(string name, Value value)
            {
                if (_values.ContainsKey(name))
                    throw new IdentifierAlreadyDefinedException(name);

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

            public Value Value
            {
                get { return _value; }
            }

            public bool IsDefined
            {
                get { return _isDefined; }
            }
        }

        private class ScopePopper : IDisposable
        {
            private readonly Scope _scope;

            public ScopePopper(Scope scope)
            {
                _scope = scope;
            }

            public void Dispose()
            {
                _scope.PopScope();
            }
        }
    }
}