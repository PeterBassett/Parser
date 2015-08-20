using System;
using System.Diagnostics;

namespace TestHelpers
{
    [DebuggerStepThrough]
    public static class RandomGenerator
    {
        [Flags]
        public enum CharacterSet
        {
            Alphabetic = 1,
            Numeric = 2,
            ExtraCharacters = 4
        }

        static readonly Random Random = new Random(DateTime.Now.Millisecond);
        private const string Alphabet = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numeric = @"0123456789";        
        private const string ExtraCharacters = @" .()%$£\/";

        public static int Int()
        {
            return Int(Random.Next());
        }

        private static int Int(int maxInt)
        {
            return Int(0, maxInt);
        }

        public static int Int(int min, int max)
        {
            return Random.Next(min, max);
        }

        public static decimal Decimal(int places)
        {
            return decimal.Parse(Int() + "." + Int(places));
        }


        private static char Character(CharacterSet characterSet)
        {
            string characters = "";

            if( characterSet.HasFlag( CharacterSet.Alphabetic ) )
                characters += Alphabet;

            if( characterSet.HasFlag( CharacterSet.Numeric ) )
                characters += Numeric;

            if( characterSet.HasFlag( CharacterSet.ExtraCharacters ) )
                characters += ExtraCharacters;

            if (characters.Length == 0)
                throw new ArgumentOutOfRangeException("characterSet");

            return characters[Random.Next(0, characters.Length)];
        }

        public static string String()
        {
            return String(Int(100));
        }

        public static string String(int exactLength)
        {
            return String(CharacterSet.Alphabetic | CharacterSet.Numeric, exactLength);
        }   

        public static string String(CharacterSet characterSet)
        {
            return String(characterSet, Int(1000));
        }

        public static string String(int minLength, int maxLength)
        {
            return String(CharacterSet.Alphabetic | CharacterSet.Numeric, minLength, maxLength);
        }

        public static string String(CharacterSet characterSet, int minLength, int maxLength)
        {
            return String(characterSet, Int(minLength, maxLength));
        }

        public static string String(CharacterSet characterSet, int exactLength)
        {
            var output = "";
            for (var i = 0; i < exactLength; i++)
                output += Character(characterSet);
            return output;
        }

        public static bool Bool()
        {
            return Int() % 2 == 0;
        }

        public static DateTime Date()
        {
            return new DateTime(Int(1990, 2020), Int(1, 12), Int(1, 28));
        }

        public static DateTime Date(DateDirection direction)
        {
            return direction == DateDirection.Past ?
                new DateTime(Int(1990, DateTime.Now.Year), Int(1, 12), Int(1, 28)) :
                new DateTime(Int(DateTime.Now.Year, 2020), Int(1, 12), Int(1, 28));
        }

        public enum DateDirection
        {
            Past = 0,
            Future = 1
        }

        public static long Long()
        {
            return (long)((Random.NextDouble() * 2.0 - 1.0) * long.MaxValue);
        }
    }
}
