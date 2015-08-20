namespace TestHelpers
{
    public static class CharacterSetExtensions 
    {
        public static bool HasFlag(this RandomGenerator.CharacterSet item, RandomGenerator.CharacterSet query) 
        { 
            return ((item & query) == query);
        }
    }   
}
