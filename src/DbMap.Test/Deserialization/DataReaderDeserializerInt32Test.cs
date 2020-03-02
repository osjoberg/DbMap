using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerInt32Test : DataReaderDeserializerTestBase<int>
    {
        public DataReaderDeserializerInt32Test() : base("SELECT {0}", int.MinValue, int.MaxValue)
        {
        }
    }
}