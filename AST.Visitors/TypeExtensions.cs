namespace AST.Visitor
{
    public static class TypeExtensions
    {
        public static bool IsNumericType(this ValueType type)
        {
            switch (type)
            {
                case ValueType.Int:
                case ValueType.Float:
                    return true;
                default:
                    return false;
            }
        }
    }
}
