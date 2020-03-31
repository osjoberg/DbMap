using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerByteTest : DataReaderDeserializerTestBase<byte>
    {
        public DataReaderDeserializerByteTest() : base("SELECT CAST({0} AS TINYINT)", byte.MinValue, byte.MaxValue)
        {
        }
    }
}