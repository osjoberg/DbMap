using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerByteArrayTest
    {
        [TestMethod]
        public void CanDeserializeNullByteArray()
        {
            DbAssert.ArrayAreEqual<byte[]>(null, "SELECT CAST(NULL AS VARBINARY)");
        }

        [TestMethod]
        public void CanDeserializeEmptyByteArray()
        {
            DbAssert.ArrayAreEqual(new byte[0], "SELECT CAST('' AS VARBINARY)");
        }

        [TestMethod]
        public void CanDeserializeOneElementByteArray()
        {
            DbAssert.ArrayAreEqual(new[] { (byte)'A' }, "SELECT CAST('A' AS VARBINARY)");
        }

        [TestMethod]
        public void CanDeserializeTwoElementByteArray()
        {
            DbAssert.ArrayAreEqual(new[] { (byte)'A', (byte)'B' }, "SELECT CAST('AB' AS VARBINARY)");
        }
    }
}
