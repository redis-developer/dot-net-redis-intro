using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Redis101Examples
{
    class SortedSetExercises
    {
        public static void sortedSetExercises(IDatabase db)
        {
            Console.WriteLine("Running Sorted Set tests...");
            // Sorted Set
            SortedSetEntry avengerSortedSetEntry = new SortedSetEntry("issue 100", 1972);
            List<SortedSetEntry> avengerIssuesList = new List<SortedSetEntry>();
            avengerIssuesList.Add(avengerSortedSetEntry);
            db.SortedSetAdd("avenger:issues", "issue 1", 1963); // Add a single entry
            db.SortedSetAdd("avengers:issues", avengerIssuesList.ToArray()); // Add multiple entries
            RedisValue[] sortedResult = db.SortedSetRangeByScore("avengers:issues", 0, -1, order: Order.Descending);
        }
    }
}