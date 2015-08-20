﻿namespace Parser.Parselets.Prefix
{
    internal class FloatParselet : LiteralParselet
    {
        protected override object Parse(string tokenValue)
        {
            return float.Parse(tokenValue);
        }
    }
}
