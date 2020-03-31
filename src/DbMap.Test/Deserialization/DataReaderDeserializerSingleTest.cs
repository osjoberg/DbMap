using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerSingleTest : DataReaderDeserializerTestBase<float>
    {
        public DataReaderDeserializerSingleTest() : base("SELECT CAST({0} AS REAL)", float.MinValue, float.MaxValue)
        {
        }
   }
}