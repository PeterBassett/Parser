using System.Collections.Generic;

namespace AST.Visitor
{
    public class Scope
    {
        private readonly Scope _parent;
        private readonly Dictionary<string, Identifier> _values;
        public Scope() : this(null)
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

            private readonly string _name;
            private readonly Value _value;
            private readonly bool _isDefined;
            private Identifier(bool unused) : this()
            {
                _name = null;
                _value = null;
                _isDefined = false;
            }

            public Identifier(string name, Value value)
            {
                _name = name;
                _value = value;
                _isDefined = true;
            }

            public string Name { get { return _name; } }
            public Value Value { get { return _value; } }
            public bool IsDefined { get { return _isDefined; } }
        }

        private Scope Parent { get { return _parent;  } }

        public void DefineIdentifier(string name, Value value)
        {            
            _values.Add(name, new Identifier(name, value));
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
}
