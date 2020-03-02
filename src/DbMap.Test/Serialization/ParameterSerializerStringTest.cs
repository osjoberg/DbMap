using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerStringTest
    {
        [TestMethod]
        public void CanSerializeNullString()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter IS NULL, 1, 0) AS BIT)", new { parameter = (string)null });
        }

        [TestMethod]
        public void CanSerializeEmptyString()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter = '', 1, 0) AS BIT)", new { parameter = "" });
        }

        [TestMethod]
        public void CanSerializeOneCharacterString()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter = 'A', 1, 0) AS BIT)", new { parameter = "A" });
        }

        [TestMethod]
        public void CanSerializeTwoCharacterString()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter = 'B', 1, 0) AS BIT)", new { parameter = "B" });
        }
    }
}