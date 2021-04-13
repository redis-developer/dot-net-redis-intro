using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Redis101Examples
{
    static class GeoLocationExercises
    {
        public static void Exercises(IDatabase db)
        {
            Console.WriteLine("Running Geo exercises...");
            // GeoLocation
            db.GeoAdd("avenger:locations", -73.984016, 40.754932, "Stark_Tower"); // Add single location

            List<GeoEntry> avengerLocations = new List<GeoEntry>();
            avengerLocations.Add(new GeoEntry(-73.968008, 40.771071, "Avengers_Mansion"));
            db.GeoAdd("avenger:locations", avengerLocations.ToArray()); // Add multiple locations

            // Get the distance between the locations in meters
            db.GeoDistance("avenger:locations", "Stark_Tower", "Avengers_Mansion", GeoUnit.Meters);

            // Get the longitude and latitude of the requested member. Multiple locations can be returned by providing a RedisValue[] containing a list of members
            db.GeoPosition("avenger:locations", "Stark_Tower");

            // Find all set members within the specified radius using the provided location as the center
            db.GeoRadius("avenger:locations", "Stark_Tower", 200, unit: GeoUnit.Kilometers);
        }
    }
}