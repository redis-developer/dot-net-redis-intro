using StackExchange.Redis;

namespace Redis101Examples
{
    class ListExercises
    {
        public static void listExercises(IDatabase db)
        {
            // List
            db.ListLeftPush("avengerList", "Iron Man"); // Insert single value
            db.ListLeftPush("avengerList", new RedisValue[] {"Wasp", "Ant Man"}); // Insert multiple values
            RedisValue aAvenger = db.ListRightPop("avengerList"); // Pop a single value
            long length = db.ListLength("avengerList"); // Get new length
        }
    }
}