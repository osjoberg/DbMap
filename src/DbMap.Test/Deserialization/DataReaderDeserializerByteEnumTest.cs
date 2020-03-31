using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerByteEnumTest : DataReaderDeserializerTestBase<DataReaderDeserializerByteEnumTest.ByteEnum>
    {
        public DataReaderDeserializerByteEnumTest() : base("SELECT CAST({0:d} AS TINYINT)", ByteEnum.MinValue, ByteEnum.MaxValue)
        {
        }

        public enum ByteEnum : byte
        {
            MinValue = byte.MinValue,
            MaxValue = byte.MaxValue
        }
    }
}