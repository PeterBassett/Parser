using System;

namespace AST.Visitor
{
    public class Value
    {
        private readonly object _value;

        public Value(object value)
        {
            _value = value;
        }

        public Value(Value value)
        {
            _value = value._value;
        }

        public int ToInt()
        {
            if (_value is int)
                return (int) _value;

            throw new InvalidCastException();
        }

        public decimal ToNumeric()
        {
            if (IsNumericType())
                return Convert.ToDecimal(_value);

            throw new InvalidCastException();
        }        

        public bool ToBoolean()
        {
            if (_value is bool)
                return (bool)_value;

            throw new InvalidCastException();
        }

        public bool IsNumericType()
        {
            if (_value == null)
                return false;

            return _value.GetType().IsNumericType();
        }

        public object ToObject()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
