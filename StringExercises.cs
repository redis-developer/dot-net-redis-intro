using System;
using Xunit;
using System.Text;
using StackExchange.Redis;

namespace Redis101Examples
{
    class StringExercises
    {
        public static void stringExercises(IDatabase db)
        {
            Console.WriteLine("Running String samples...");

            // 1. Set and get a Redis string.
            db.StringSet("planet:0", "Mercury");
            RedisValue response = db.StringGet("planet:0");
            Assert.Equal("Mercury", response);

            // 2. Represent a string as raw bytes.
            byte[] key = Encoding.UTF8.GetBytes("key"), value = Encoding.UTF8.GetBytes("value");
            db.StringSet(key, value);
            byte[] rawValue = db.StringGet(key);

            // 3. Perform bitwise operations.
            db.StringSetBit("bitset1", 16, true); // Set the bit at offset 16 to 1
            bool isBitSet = db.StringGetBit("bitset1", 16); // Returns true or false for the value of the bit at offset 16
            Assert.True(isBitSet);

            // 4. Create two new bitfields
            db.StringSet("bitfield0", new byte[] {3}); // Create new bitfields using binary safe string
            db.StringSet("bitfield1", new byte[] {6});
            // The available bitwise logical operators are `and`, `or`, `xor`, and `not`.
            long resultLength = db.StringBitOperation(Bitwise.And, "resultBitfield", "bitfield0", "bitfield1");
            Assert.Equal(1, resultLength);

            byte[] resultValue = (byte[]) db.StringGet("resultBitfield"); // Resulting bitfield can be read back as a byte array
            long bitCount = db.StringBitCount("resultBitfield");
            Assert.Equal(1, bitCount);
        }
    }
}