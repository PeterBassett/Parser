using System.Collections.Generic;
using System.Linq;

namespace TestHelpers
{
    public static class Extensions
    {
        public static T TakeRandomSingle<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Skip(RandomGenerator.Int(0, enumerable.Count() - 1)).Take(1).Single();
        }
    }
}
