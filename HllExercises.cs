using System;
using StackExchange.Redis;
using Xunit;

namespace Redis101Examples
{
    static class HllExercises
    {
        public static void Exercises(IDatabase db)
        {
            Console.WriteLine("Running HyperLogLog exercises...");
            // Hyperloglog
            db.HyperLogLogAdd("unique:landingpage:hits", "userId:123");
            db.HyperLogLogAdd("unique:landingpage:hits", "userId:456");
            var cardinality = db.HyperLogLogLength("unique:landingpage:hits");
            Assert.Equal(2, cardinality);
        }
    }
}