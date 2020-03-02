using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerCharArrayTest
    {
        [TestMethod]
        public void CanDeserializeNullCharArray()
        {
            DbAssert.CollectionAreEqual<char[]>(null, "SELECT CAST(NULL AS VARCHAR)");
        }

        [TestMethod]
        [ExpectedException(typeof(EntryPointNotFoundException))]
        public void CanDeserializeEmptyCharArray()
        {
            DbAssert.CollectionAreEqual(new char[0], "SELECT CAST('' AS VARCHAR)");
        }

        [TestMethod]
        [ExpectedException(typeof(EntryPointNotFoundException))]
        public void CanDeserializeOneElementCharArray()
        {
            DbAssert.CollectionAreEqual(new[] { 'A' }, "SELECT CAST('A' AS VARCHAR)");
        }

        [TestMethod]
        [ExpectedException(typeof(EntryPointNotFoundException))]
        public void CanDeserializeTwoElementCharArray()
        {
            DbAssert.CollectionAreEqual(new[] { 'A', 'B' }, "SELECT CAST('AB' AS VARCHAR)");
        }
    }
}
