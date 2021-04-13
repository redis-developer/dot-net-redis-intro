using StackExchange.Redis;

namespace Redis101Examples
{
    static class Program
    {
        static void Main()
        {
            // Configuration options and patterns
            string redisConfiguration = "localhost:6379"; // Store *basic* information in a string
            var options = ConfigurationOptions.Parse(redisConfiguration); // create a ConfigurationOptions instance
            options.AllowAdmin = true; // and set specific details with options
            options.Ssl = false;
            options.ConnectRetry = 1;
            options.HighPrioritySocketThreads = true;
            
            // Multiplexer is intended to be reused
            ConnectionMultiplexer redisMultiplexer = ConnectionMultiplexer.Connect(options);
            
            // The database reference is a lightweight passthrough object intended to be used and discarded
            IDatabase db = redisMultiplexer.GetDatabase();
            
            // All Redis commands and data types are supported and available through the API

            StringExercises.Exercises(db);

            HashExercises.Exercises(db);

            ListExercises.Exercises(db);

            SetExercises.Exercises(db);

            SortedSetExercises.Exercises(db);

            GeoLocationExercises.Exercises(db);

            HllExercises.Exercises(db);

            StreamExercises.Exercises(db);

            PubSubExercises.Exercises(redisMultiplexer);
        }
    }
}