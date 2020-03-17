using System.Linq;

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
            var expected = Enumerable.Range(0, 1000).Select(Large.Create).ToList();

            DbAssert.CollectionAreEqual(expected, "SELECT * FROM Large");
        }
    }
}
