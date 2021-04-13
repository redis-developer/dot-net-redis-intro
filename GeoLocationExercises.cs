using System;
using System.Collections.Generic;
using StackExchange.Redis;
using Xunit;

namespace Redis101Examples
{
    static class GeoLocationExercises
    {
        public static void Exercises(IDatabase db)
        {
            Console.WriteLine("Running Geo exercises...");
            // Geo sets are "Sorted Sets"
            // 52-bit integers for geohash
            // GeoLocation
            db.GeoAdd("avenger:locations", -73.984016, 40.754932, "Stark_Tower"); // Add single location
            
            List<GeoEntry> avengerLocations = new List<GeoEntry>
            {
                new(-73.968008, 40.771071, "Avengers_Mansion")
            };
            db.GeoAdd("avenger:locations", avengerLocations.ToArray()); // Add multiple locations

            // Get the distance between the locations in meters
            //db.GeoDistance("avenger:locations", "Stark_Tower", "Avengers_Mansion", GeoUnit.Meters);
            // GeoUnit.Meters is default, so this result is the same. Other options are Kilometers, Miles, Feet
            var metersBetween = db.GeoDistance("avenger:locations", "Stark_Tower", "Avengers_Mansion");
            Assert.Equal(2245.3769, metersBetween);
            
            // Get the longitude and latitude of the requested member. Multiple locations can be returned by providing a RedisValue[] containing a list of members
            var geoPosition = db.GeoPosition("avenger:locations", "Stark_Tower");
            // Converting from GEOHASH to GEOPOS can provide different outputs than inputs, so we use an approximation
            Assert.Contains("-73.98401", Convert.ToDouble(geoPosition.Value.Longitude).ToString());
            Assert.Contains("40.75493", Convert.ToDouble(geoPosition.Value.Latitude).ToString());

            // Find all set members within the specified radius using the provided location as the center
            var results = db.GeoRadius("avenger:locations", "Stark_Tower", 3, unit: GeoUnit.Kilometers);
            foreach (var result in results)
            {
                Console.WriteLine("In Radius: " +result);
            }
        }
    }
}