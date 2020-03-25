using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerStringTest
    {
        [TestMethod]
        public void CanDeserializeNullString()
        {
            DbAssert.AreEqual<string>(null, "SELECT CAST(NULL AS NVARCHAR(10))");
        }

        [TestMethod]
        public void CanDeserializeEmptyString()
        {
            DbAssert.AreEqual("", "SELECT ''");
        }

        [TestMethod]
        public void CanDeserializeOneCharacterString()
        {
            DbAssert.AreEqual("A", "SELECT 'A'");
        }

        [TestMethod]
        public void CanDeserializeTwoCharacterString()
        {
            DbAssert.AreEqual("AB", "SELECT 'AB'");
        }
    }
}