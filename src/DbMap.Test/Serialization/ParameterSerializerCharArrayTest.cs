using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerCharArrayTest
    {
        [TestMethod]
        public void CanSerializeNullCharArray()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter IS NULL, 1, 0) AS BIT)", new { parameter = (char[])null });
        }

        [TestMethod]
        public void CanSerializeEmptyCharArray()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter = '', 1, 0) AS BIT)", new { parameter = new char[] { } });
        }

        [TestMethod]
        public void CanSerializeOneElementCharArray()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter = 'A', 1, 0) AS BIT)", new { parameter = new[] { 'A' } });
        }

        [TestMethod]
        public void CanSerializeTwoElementCharArray()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter = 'AB', 1, 0) AS BIT)", new { parameter = new[] { 'A', 'B' } });
        }
    }
}
