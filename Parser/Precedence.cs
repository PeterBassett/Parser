namespace Parser
{
    public enum Precedence
    {
        // Ordered in increasing precedence.        
        Assignment = 1,
        Conditional,
        Logical,
        Equality,
        Comparison,
        Sum,
        Product,
        Exponent,
        Unary,
        Prefix,
        Postfix,        
        Call//,
        //Assignment
    }
}
