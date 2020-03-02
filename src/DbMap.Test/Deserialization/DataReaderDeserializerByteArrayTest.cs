using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerByteArrayTest
    {
        [TestMethod]
        public void CanDeserializeNullByteArray()
        {
            DbAssert.CollectionAreEqual<byte[]>(null, "SELECT CAST(NULL AS VARBINARY)");
        }

        [TestMethod]
        public void CanDeserializeEmptyByteArray()
        {
            DbAssert.CollectionAreEqual(new byte[0], "SELECT CAST('' AS VARBINARY)");
        }

        [TestMethod]
        public void CanDeserializeOneElementByteArray()
        {
            DbAssert.CollectionAreEqual(new[] { (byte)'A' }, "SELECT CAST('A' AS VARBINARY)");
        }

        [TestMethod]
        public void CanDeserializeTwoElementByteArray()
        {
            DbAssert.CollectionAreEqual(new[] { (byte)'A', (byte)'B' }, "SELECT CAST('AB' AS VARBINARY)");
        }
    }
}
