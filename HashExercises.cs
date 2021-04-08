using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Redis101Examples
{
    class HashExercises
    {
        public static void hashExercises(IDatabase db)
        {
            Console.WriteLine("Running Hash exercises...");
            // Hash
            db.HashSet("avenger:1", "name", "Tony Stark"); // Set a single has field
            List<HashEntry> fields = new List<HashEntry>();
            fields.Add(new HashEntry("alias", "Iron Man"));
            fields.Add(new HashEntry("address", "Stark Tower"));
            db.HashSet("avenger:1", fields.ToArray()); // Set multiple has fields
            RedisValue singleField = db.HashGet("avenger:1", "name");
            HashEntry[] entireHashset = db.HashGetAll("avenger:1");
        }
    }
}