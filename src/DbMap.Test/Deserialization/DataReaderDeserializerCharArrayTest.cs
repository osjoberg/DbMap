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
            DbAssert.ArrayAreEqual<char[]>(null, "SELECT CAST(NULL AS NVARCHAR(10))");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CanDeserializeEmptyCharArray()
        {
            DbAssert.ArrayAreEqual(new char[0], "SELECT CAST('' AS NVARCHAR(10))");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CanDeserializeOneElementCharArray()
        {
            DbAssert.ArrayAreEqual(new[] { 'A' }, "SELECT CAST('A' AS NVARCHAR(10))");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CanDeserializeTwoElementCharArray()
        {
            DbAssert.ArrayAreEqual(new[] { 'A', 'B' }, "SELECT CAST('AB' AS NVARCHAR(10))");
        }
    }
}
