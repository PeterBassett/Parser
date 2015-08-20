namespace Parser.Parselets.Prefix
{
    internal class BooleanParselet : LiteralParselet
    {
        protected override object Parse(string tokenValue)
        {
            return bool.Parse(tokenValue);
        }
    }
}
