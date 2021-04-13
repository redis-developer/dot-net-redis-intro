using System;
using Xunit;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Redis101Examples
{
    static class HashExercises
    {
        public static void Exercises(IDatabase db)
        {
            Console.WriteLine("Running Hash exercises...");
            db.KeyDelete("avenger:1");

            // 1. Create a new Redis hash, with two fields.
            db.HashSet("avenger:1", "name", "Tony Stark");
            db.HashSet("avenger:1", "age", "41");

            // 2. Get the value of a field from a Hash.
            //    What's the name of avenger 1?
            RedisValue name = db.HashGet("avenger:1", "name");
            Assert.Equal("Tony Stark", name);


            // 3. Write multiple fields all at once.
            List<HashEntry> fields = new List<HashEntry>();
            fields.Add(new HashEntry("alias", "Iron Man"));
            fields.Add(new HashEntry("address", "Stark Tower"));
            db.HashSet("avenger:1", fields.ToArray());


            // 4. Return the entire hash
            HashEntry[] entireHashset = db.HashGetAll("avenger:1");
        }
    }
}