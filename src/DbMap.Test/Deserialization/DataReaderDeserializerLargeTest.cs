using DbMap.Benchmark;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerLargeTest
    {
        [TestMethod]
        public void CanDeserializeUserType()
        {
            DbAssert.AreEqual(Large.Create(0), "SELECT TOP 1 * FROM Large", false);
        }
    }
}
