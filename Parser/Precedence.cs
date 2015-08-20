namespace Parser
{
    public enum Precedence
    {
        // Ordered in increasing precedence.
        Assignment = 1,
        Conditional,
        Sum,
        Product,
        Exponent,
        Prefix,
        Postfix,
        Call
    }
}
