using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Redis101Examples
{
    class SetExercises
    {
        public static void setExercises(IDatabase db)
        {
            Console.WriteLine("Running Set tests...");
            // Set
            db.SetAdd("avengers", "Iron Man"); // add a single item to the set
            List<RedisValue> avengers = new List<RedisValue>();
            avengers.Add("Hulk");
            avengers.Add("Thor");
            avengers.Add("Ant Man");
            db.SetAdd("avengers", avengers.ToArray()); // add multiple items to the set
            bool setResults = db.SetContains("avengers", "Iron Man"); // true
            RedisValue[] anotherResult = db.SetMembers("avengers"); // all members of the set
        }
    }
}