using System;
using StackExchange.Redis;

namespace Redis101Examples
{
    static class StreamExercises
    {
        public static void Exercises(IDatabase db)
        {
            Console.WriteLine("Running Stream exercises...");
            db.KeyDelete("hero:organization");

            // Publish to a stream.  Not supplying a streamId causes one to be generated by Redis using the timestamp default.
            // If the stream does not exist then it is created when StreamAdd is called.
            // Differences from Pub/Sub
            // - Every client (consumer) will receive a copy of any item added to the stream
            // - All messages appended to a stream will be available for future retrieval unless explicitly deleted
            // - Consumer Groups provide process acknowledgement, inspection of pending items, message claiming, and per-client private history, which is unavailable to pub/sub

            // 1. Add an entry with a single name-value pair to the stream
            RedisValue streamMessageId = db.StreamAdd("hero:organization", "name", "Wasp", messageId: "*");

            // Add an entry with multiple name-value pairs to the stream
            var streamPairs1 = new NameValueEntry[]
            {
                new NameValueEntry("name", "Hulk"),
                new NameValueEntry("affiliation", "Avengers")
            };

            var streamPairs2 = new NameValueEntry[]
            {
                new NameValueEntry("name", "Iron Man"),
                new NameValueEntry("affiliation", "Avengers")
            };

            RedisValue otherMessageSteamId1 = db.StreamAdd("hero:organizations", streamPairs1, messageId: "*");
            RedisValue otherMessageSteamId2 = db.StreamAdd("hero:organizations", streamPairs2, messageId: "*");

            // 2. Read from a stream. This will return all messages from the ID unti 
            //    the end of the stream unless a count is given
            StreamEntry[] firstMessage = db.StreamRead("hero:organizations", "0", count: 1);

            // StreamRange is used to read a consecutive set of IDs.  The characters `+` and `-` are used to indicate the minimum and maximum possible IDs.
            // If no value is passed to the respective parameters the entire stream will be read.
            StreamEntry[] rangeOfMessages = db.StreamRange("hero:organizations", minId: "0", maxId: "1");

            // StreamInfo can be used to gather information about a stream.
            StreamInfo info = db.StreamInfo("hero:organizations");
            Console.WriteLine("Stream length " + info.Length);

            // ConsumerGroups
            // Consumer groups act as an logical endpoint for a stream. The individual consumers are then served by the consumer group.
            // Using a consumer group provides certain guarantees
            // - Each message will only be sent once to a single client
            // - Consumers are identified within the consumer group by a unique case-sensitive string which allows the consumer group to retain state in the event of a consumer disconnect
            // - The consumer group maintains a state of the last message ID processed by a consumer which allows pending messages for a consumer to be process in order
            // - Consuming pending messages requires an explicit acknowledgement which gives the ability to enforce processing guarantees
            // - Consumer groups track pending messages, where pending is defined as, messages delivered to a consumer but know yet acknowledged. This mechanism means that each consumer sees only its own message history

            // CreateConsumerGroup
            // If the referenced stream does not exist when the consumer group is created, this will create the stream
            // If the first visible message to the new group requires a position other than the beginning of the stream or the first new message after joining,
            // position can be specified by giving a specific message ID instead of a `StreamPosition` constant.

            // Synchronous
            bool created = db.StreamCreateConsumerGroup("hero:organizations", "hero_consumer_group1", StreamPosition.Beginning);

            // A named consumer is created if it doesn't already exist in the consumer group.  `>` is a special character that means, "read not delivered to any consumer"
            // Count limits the returned messages to the count quantity.  If a message ID is passed instead of the `>` character, the pending messages for that consumer are 
            // searched and if a message with a matching ID is found, that StreamEntry is returned.
            StreamEntry[] consumer1_messages =
                db.StreamReadGroup("hero:organizations", "hero_consumer_group1", "shield", ">", count: 1);
            // Read a message for a second consumer in the same consumer group
            StreamEntry[] consumer2_messages =
                db.StreamReadGroup("hero:organizations", "hero_consumer_group1", "AIM", ">", count: 1);

            // StreamPending
            // StreamPending returns high level state information about a given consumers messages
            StreamPendingInfo pending_messages = db.StreamPending("hero:organizations", "hero_consumer_group1");
            var count = pending_messages.PendingMessageCount;
            RedisValue messageId = pending_messages.LowestPendingMessageId;

            Console.WriteLine("Stream messageId = {0}", messageId);
            // StreamPendingMessages
            // StreamPendingMessages shows detailed message information about pending messages for a given consumer
            StreamPendingMessageInfo[] pendingMessage =
                db.StreamPendingMessages("hero:organizations", "hero_consumer_group1", 1, "shield", messageId);
            RedisValue grp_name = pendingMessage[0].ConsumerName;

            // StreamAcknowledge
            // This will send an ACK to the consumer group for the given message ID, removing it from the pending messages for the consumer
            var acknowledgedCount = db.StreamAcknowledge("hero:organizations", "hero_consumer_group1", messageId);

            // StreamDeleteConsumer
            // This will delete the given consumer from the consumer group.  There is a separate delete commands for consumer groups, and stream messages
            var pendingAIMmessages = db.StreamDeleteConsumer("hero:organizations", "hero_consumer_group1", "AIM");
            var pending_shield_messages = db.StreamDeleteConsumer("hero:organizations", "hero_consumer_group1", "shield");

            // StreamDeleteConsumerGroup
            bool deleted = db.StreamDeleteConsumerGroup("hero:organizations", "hero_consumer_group1");
        }
    }
}