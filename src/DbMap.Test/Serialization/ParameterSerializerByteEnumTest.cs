using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerByteEnumTest : ParameterSerializerTestBase<ParameterSerializerByteEnumTest.ByteEnum>
    {
        public ParameterSerializerByteEnumTest() : base("SELECT CAST(IIF(@parameter = {0:d}, 1, 0) AS BIT)", ByteEnum.MinValue, ByteEnum.MaxValue)
        {
        }

        public enum ByteEnum : byte
        {
            MinValue = byte.MinValue,
            MaxValue = byte.MaxValue
        }
    }
}