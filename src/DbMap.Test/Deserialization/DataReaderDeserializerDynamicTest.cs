using System.Linq;

using DbMap.Benchmark;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerDynamicTest
    {
        [TestMethod]
        public void CanDeserializeDynamic()
        {
            var expected = Enumerable.Range(0, 1000).Select(Large.Create).Cast<dynamic>().ToList();

            DbAssert.CollectionAreEqual(expected, "SELECT * FROM Large");
        }
    }
}
