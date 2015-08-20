namespace Parser
{
    public enum Precedence
    {
        // Ordered in increasing precedence.
        Assignment = 1,
        Logical,
        Equality,
        Comparison,
        Sum,
        Product,
        Exponent,
        Prefix,
        Postfix,
        Call
    }
}
