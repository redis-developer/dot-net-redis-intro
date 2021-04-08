using StackExchange.Redis;

namespace Redis101Examples
{
    class HllExercises
    {
        public static void hllExercises(IDatabase db)
        {
            // Hyperloglog
            db.HyperLogLogAdd("unique:landingpage:hits", "userId:123");
            db.HyperLogLogAdd("unique:landingpage:hits", "userId:456");
            long cardinality = db.HyperLogLogLength("unique:landingpage:hits");
        }
    }
}