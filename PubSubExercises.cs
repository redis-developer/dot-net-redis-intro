using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Xunit;

namespace Redis101Examples
{
    static class PubSubExercises
    {
        public static void Exercises(ConnectionMultiplexer redisMultiplexer)
        {
            Console.WriteLine("Running Pub/Sub exercises...");
            // PubSub
            // Async vs Sync handlers
            // Sequential vs Concurrent message handling
            // Differences from Streams
            // - Pub/Sub has no history, messages are fire and forget

            // Similar to the IDatabase object, this is a lightweight pass-through object to be used and discarded
            ISubscriber subscription = redisMultiplexer.GetSubscriber();
            
            // subscribing to a channel V1
            subscription.Subscribe("customer:request:events",
                (channel, message) => { Console.WriteLine("Pub/Sub message {0}", message); });
            
            subscription.Publish("customer:request:events", "New Customer Request 1");

            // subscribing to a channel; v2
            // Synchronous - messages are processed in the order received but may delay each other and code hampers scalability
            subscription.Subscribe("customer:completed:requests").OnMessage(message =>
            {
                Console.WriteLine("Do some work when message, {0}, is received.", message);
            });
            
            subscription.Publish("customer:completed:requests", "Completed 1");
            subscription.Publish("customer:completed:requests", "Completed 2");
            
            async Task<ChannelMessage> AsyncMessageTask(ChannelMessage message)
            {
                Console.WriteLine("Do some work when message, {0}, is received.", message);
                await Task.Delay(1000);
                return message;
            }
            
            // subscribing to a channel; v2
            // Asynchronous - messages are published concurrently and the code is a more scalable 
            subscription.Subscribe("customer:async:requests").OnMessage(  message =>
            {
                var result =  AsyncMessageTask(message);
                Assert.NotNull(result);
            });

            subscription.Publish("customer:async:requests", "Completed 1");
            subscription.Publish("customer:async:requests", "Completed 2");
            
            
            subscription.Publish("customer:request:events", "Sync 1");
            subscription.Publish("customer:request:events", "Sync 2");
            subscription.Publish("customer:request:events", "Sync 3");
            subscription.Publish("customer:request:events", "Sync 4");
            
        }
    }
}