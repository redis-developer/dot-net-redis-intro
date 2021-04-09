using System;
using Xunit;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Redis101Examples
{
    class SortedSetExercises
    {
        public static void sortedSetExercises(IDatabase db)
        {
            Console.WriteLine("Running Sorted Set tests...");
            db.KeyDelete("avengers:strength");

            // 1. Create a sorted set entry. These entries consist of
            //    a unique value and a score.
            // Suppose we're modeling the "strength" of each Avenger.
            db.SortedSetAdd("avengers:strength", "Iron Man", 100);
            db.SortedSetAdd("avengers:strength", "Hulk", 250);

            // 2. Add many Avengers at once.
            List<SortedSetEntry> avengerIssuesList = new List<SortedSetEntry>();
            avengerIssuesList.Add(new SortedSetEntry("Spiderman", 95));
            avengerIssuesList.Add(new SortedSetEntry("Vision", 47));
            avengerIssuesList.Add(new SortedSetEntry("Quicksilver", 118));
            db.SortedSetAdd("avengers:strength", avengerIssuesList.ToArray());

            // 3. Let's find the weakest and strongest Avengers.
            //    A super efficient operation (O(log n)) even with sets
            //    containing millions of elements.
            SortedSetEntry[] weakestResults = db.SortedSetRangeByRankWithScores("avengers:strength", 0, 0, order: Order.Ascending);
            var weakest = (SortedSetEntry)weakestResults.GetValue(0);
            Assert.Equal("Vision", weakest.Element);

            SortedSetEntry[] strongestResults = db.SortedSetRangeByRankWithScores("avengers:strength", 0, 0, order: Order.Descending);
            var strongest = (SortedSetEntry)strongestResults.GetValue(0);
            Assert.Equal("Hulk", strongest.Element);
        }
    }
}