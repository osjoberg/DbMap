using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerDecimalTest : DataReaderDeserializerTestBase<decimal>
    {
        public DataReaderDeserializerDecimalTest() : base("SELECT CAST({0} AS DECIMAL(38,0))", decimal.MinValue, decimal.MaxValue)
        {
        }
    }
}