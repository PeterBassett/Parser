using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using AST.Expressions.Function;

namespace AST.Visitor
{
    public class Value
    {
        private readonly ValueType _type;
        private readonly object _value;

        public Value()
        {
            _type = ValueType.Unit;
        }

        public static Value FromObject(object value)
        {
            switch (GetValueType(value))
            {
                case ValueType.Int:
                    return new Value(Convert.ToInt64(value));
                case ValueType.Float:
                    return new Value(Convert.ToDouble(value));
                case ValueType.Boolean:
                    return new Value(Convert.ToBoolean(value));
                case ValueType.String:
                    return new Value(Convert.ToString(value));
                case ValueType.Function:
                    return new Value((FunctionDefinitionExpr)value);
                case ValueType.Unit:
                    return new Value();
                default:
                    throw new InvalidCastException("Value can not represent the supplied type " + value.GetType().FullName);
            }            
        }

        public static ValueType GetValueType(object value)
        {
            if(value == null)
                return ValueType.Unit;

            if(value is bool)
                return ValueType.Boolean;

            if (value is long)
                return ValueType.Int;

            if (value is int)
                return ValueType.Int;

            if (value is float)
                return ValueType.Float;

            if (value is double)
                return ValueType.Float;

            if (value is string)
                return ValueType.String;

            if (value is FunctionDefinitionExpr)
                return ValueType.Function;

            throw new InvalidCastException("Value can not represent the supplied type " + value.GetType().FullName);
        }

        public Value(Value value)
        {
            _value = value._value;
            _type = value._type;
        }

        public Value(long value)
        {
            _value = value;
            _type = ValueType.Int;
        }

        public Value(double value)
        {
            _value = value;
            _type = ValueType.Float;
        }

        public Value(string value)
        {
            _value = value;
            _type = ValueType.String;
        }

        public Value(bool value)
        {
            _value = value;
            _type = ValueType.Boolean;
        }

        public Value(FunctionDefinitionExpr func)
        {
            _value = func;
            _type = ValueType.Function;
        }

        public ValueType Type
        {
            get { return _type; }            
        }

        public long ToInt()
        {
            return Convert.ToInt64(_value);
        }

        public double ToDouble()
        {
            return Convert.ToDouble(_value);
        }        

        public bool ToBoolean()
        {
            return Convert.ToBoolean(_value);
        }

        public bool IsNumericType()
        {
            if (_value == null)
                return false;

            return Type.IsNumericType();
        }

        public object ToObject()
        {
            return _value;
        }

        public bool IsFunction { get { return _type == ValueType.Function; } }

        public FunctionDefinitionExpr ToFuntion() { return _value as FunctionDefinitionExpr; }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
