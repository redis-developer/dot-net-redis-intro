using System;
using Xunit;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Redis101Examples
{
    static class SetExercises
    {
        public static void Exercises(IDatabase db)
        {
            Console.WriteLine("Running Set exercises...");
            db.KeyDelete("avengers");

            // 1. Add items to a set.
            db.SetAdd("avengers", "Iron Man");
            // Initialize a new collection with duplicates
            List<RedisValue> avengers = new List<RedisValue>
            {
                "Hulk",
                "Hulk",
                "Hulk",
                "Thor",
                "Ant Man"
            };
            long numAdded = db.SetAdd("avengers", avengers.ToArray());
            Assert.Equal(3, numAdded);

            // 2. How many members does this set contain?
            long setLength = db.SetLength("avengers");
            Assert.Equal(4, setLength);

            // 3. Check to see of a set contains a member (a O(1) operation).
            bool containsIronMan = db.SetContains("avengers", "Iron Man");
            Assert.True(containsIronMan);

            bool containsSuperman = db.SetContains("avengers", "Superman");
            Assert.False(containsSuperman);
        }
    }
}