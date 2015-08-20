namespace Parser.Parselets.Prefix
{
    internal class IntegerParselet : LiteralParselet
    {
        protected override object Parse(string tokenValue)
        {
            return int.Parse(tokenValue);
        }
    }
}
