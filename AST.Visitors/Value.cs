using System;

namespace AST.Visitor
{
    public class Value
    {
        private readonly object _value;

        public Value()
        {            
        }

        public Value(object value)
        {
            _value = value;
        }

        public Value(Value value)
        {
            _value = value._value;
        }

        public TypeCode GetTypeCode()
        {
            switch (Type.GetTypeCode(_value.GetType()))
            {
                /* case TypeCode.Empty:
                     return TypeCode.Empty;
                 case TypeCode.Object:
                 case TypeCode.DBNull:
                     return TypeCode.Object;
                 case TypeCode.Char:
                     return TypeCode.Char;
                 case TypeCode.DateTime:
                     return TypeCode.DateTime;
                      * */

                case TypeCode.Boolean:
                    return TypeCode.Boolean;
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return TypeCode.Int64;
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return TypeCode.Double;
                case TypeCode.String:
                    return TypeCode.String;
                default:
                    throw new NotSupportedException("Invalid type {0}" + _value.GetType().Name);
            }
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
