using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerUnsupportedTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void DoesNotDeserializeUnsupportedTypes()
        {
            DbAssert.AreEqual(new UserType(), "SELECT CAST(1 AS BIT) AS Value", false);
        }

        private class UserType
        {
            public UnsupportedType Value { get; set; }
        }

        private struct UnsupportedType
        {
        }
    }
}
