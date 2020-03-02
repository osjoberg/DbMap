using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerStringTest
    {
        [TestMethod]
        public void CanDeserializeNullString()
        {
            DbAssert.AreEqual<string>(null, "SELECT NULL");
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