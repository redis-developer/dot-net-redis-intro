using System.Text;
using StackExchange.Redis;

namespace Redis101Examples
{
    class StringExercises
    {
        public static void stringExercises(IDatabase db)
        {
            // Basic String operations 
            // Redis String
            db.StringSet("key", "value");
            RedisValue stringVal = db.StringGet("key");

            // Strings as raw binary data 
            byte[] key = Encoding.UTF8.GetBytes("key"), value = Encoding.UTF8.GetBytes("value");
            db.StringSet(key, value);
            byte[] rawValue = db.StringGet(key);

            // Bitwise Operations
            db.StringSetBit("bitset1", 16, true); // Set the bit at offset 16 to 1
            db.StringGetBit("bitset1", 16); // Returns true or false for the value of the bit at offset 16

            db.StringSet("bitfield0", new byte[] {3}); // Create new bitfields using binary safe string
            db.StringSet("bitfield1", new byte[] {6});
            // The available bitwise logical operators are `and`, `or`, `xor`, and `not`.
            long resultLength = db.StringBitOperation(Bitwise.And, "resultBitfield", "bitfield0", "bitfield1");
            byte[] resultValue = (byte[]) db.StringGet("resultBitfield"); // Resulting bitfield can be read back as a byte array
            long bitCount = db.StringBitCount("resultBitfield");
        }
    }
}