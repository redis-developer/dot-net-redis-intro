using System;
using StackExchange.Redis;

namespace Redis101Examples
{
    class PubSubExercises
    {
        public static void pubSubExercises(ConnectionMultiplexer redisMultiplexer)
        {
            // PubSub
            // Async vs Sync handlers
            // Sequential vs Concurrent message handling
            // Differences from Streams
            // - Pub/Sub has no history, messages are fire and forget
            // - 

            // Similar to the IDatabase object, this is a lightweight pass-through object to be used and discarded
            ISubscriber subscription = redisMultiplexer.GetSubscriber();

            // subscribing to a channel V1
            subscription.Subscribe("customer:request:events",
                (channel, message) => { Console.WriteLine("Something that does something in response to {0}", message); });

            // subscribing to a channel; v2
            // Synchronous - messages are processed in the order received but may delay each other and code hampers scalability
            subscription.Subscribe("customer:completed:requests").OnMessage(message =>
            {
                Console.WriteLine("Do some work when message, {0}, is received.", message);
            });

            // subscribing to a channel; v2
            // Asynchronous - messages are published concurrently and the code is a more scalable 
            subscription.Subscribe("customer:completed:requests").OnMessage(async message =>
            {
                Console.WriteLine("Do some work when message, {0}, is received.", message);
            });

            // publish to a channel (same in v1 and v2)
            subscription.Publish("customer:request:events", "New Customer Request");
        }
    }
}