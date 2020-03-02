using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerInt16Test : DataReaderDeserializerTestBase<short>
    {
        public DataReaderDeserializerInt16Test() : base("SELECT CAST({0} AS SMALLINT)", short.MinValue, short.MaxValue)
        {
        }
    }
}