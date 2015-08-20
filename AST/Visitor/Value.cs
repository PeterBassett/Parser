using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool IsNumericType()
        {
            if (_value == null)
                return false;

            return IsNumericType(_value.GetType());
        }

        public static bool IsNumericType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
