using System;
using Xunit;
using StackExchange.Redis;

namespace Redis101Examples
{
    static class ListExercises
    {
        public static void Exercises(IDatabase db)
        {
            Console.WriteLine("Running List exercises...");
            db.KeyDelete("avengerList");

            // 1. Add an item to a list.
            db.ListLeftPush("avengerList", "Iron Man");
            db.ListLeftPush("avengerList", new RedisValue[] {"Wasp", "Ant Man"});

            long length = db.ListLength("avengerList");
            Assert.Equal(3, length);

            // 2. Pop a value from the right-hand size of the list.
            RedisValue avenger = db.ListRightPop("avengerList");
            Assert.Equal("Iron Man", avenger);

            // 3. Check the new length of the list.
            long newLength = db.ListLength("avengerList");
            Assert.Equal(2, newLength);
        }
    }
}