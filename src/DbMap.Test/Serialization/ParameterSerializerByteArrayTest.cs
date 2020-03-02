using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerByteArrayTest
    {
        [TestMethod]
        public void CanSerializeNullByteArray()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter IS NULL, 1, 0) AS BIT)", new { parameter = (byte[])null });
        }

        [TestMethod]
        public void CanSerializeEmptyByteArray()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(CAST(@parameter AS VARCHAR) = '', 1, 0) AS BIT)", new { parameter = new byte[] { } });
        }

        [TestMethod]
        public void CanSerializeOneElementByteArray()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(CAST(@parameter AS VARCHAR) = 'A', 1, 0) AS BIT)", new { parameter = new[] { (byte)'A' } });
        }

        [TestMethod]
        public void CanSerializeTwoElementByteArray()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(CAST(@parameter AS VARCHAR) = 'AB', 1, 0) AS BIT)", new { parameter = new[] { (byte)'A', (byte)'B' } });
        }
    }
}
