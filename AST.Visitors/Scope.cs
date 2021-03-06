﻿using System;
using System.Collections.Generic;
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
            _scope.DefineIdentifier(name, value, value.Type);
        }

        public void DefineIdentifier(string name, Value value, string type)
        {
            _scope.DefineIdentifier(name, value, type);
        }

        public void AssignIdentifierValue(string identifier, Value value)
        {
            _scope.AssignIdentifierValue(identifier, value);
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
                    _scope.DefineIdentifier(arguments[i].Name.Name, values[i], values[i].Type.ToString());
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

            public void DefineIdentifier(string name, Value value, ValueType type)
            {
                DefineIdentifier(name, value, type.ToString());
            }

            public void DefineIdentifier(string name, Value value, string type)
            {
                if (_values.ContainsKey(name))
                    throw new IdentifierAlreadyDefinedException(name);

                _values.Add(name, new Identifier(value, type));
            }

            public void AssignIdentifierValue(string name, Value value)
            {
                ScopeLevel scope = this;

                while (scope != null && !scope._values.ContainsKey(name))
                    scope = scope.Parent;

                if(scope == null)
                    throw new UndefinedIdentifierException(name);

                //if (_values[name].Type != value.Type)
                //    throw new InvalidCastException();

                scope._values[name] = new Identifier(value, scope._values[name].Type);
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
            private ValueType _type;
            private string _typeName;            

            private Identifier(bool unused)
                : this()
            {
                _isDefined = false;
                _value = null;
                _type = ValueType.Unit;
            }

            public Identifier(Value value, string typeName)
            {
                _value = value;
                _isDefined = true;
                _typeName = typeName;
                _type = ValueType.Unit;
            }

            public Identifier(Value value, ValueType type)
            {
                _value = value;
                _isDefined = true;
                _typeName = null;
                _type = type;
            }

            public Value Value
            {
                get { return _value; }
            }

            public ValueType Type
            {
                get { return _type; }
            }

            public string TypeName
            {
                get { return _typeName; }
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