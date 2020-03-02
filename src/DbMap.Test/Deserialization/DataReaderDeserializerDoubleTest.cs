using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerDoubleTest : DataReaderDeserializerTestBase<double>
    {
        public DataReaderDeserializerDoubleTest() : base("SELECT CAST({0} AS FLOAT)", double.MinValue, double.MaxValue)
        {
        }
    }
}