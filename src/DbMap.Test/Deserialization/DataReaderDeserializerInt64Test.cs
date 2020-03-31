using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerInt64Test : DataReaderDeserializerTestBase<long>
    {
        public DataReaderDeserializerInt64Test() : base("SELECT CAST({0} AS BIGINT)", long.MinValue, long.MaxValue)
        {
        }
    }
}