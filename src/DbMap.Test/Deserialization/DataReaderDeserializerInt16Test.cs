using System;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerInt16Test : DataReaderDeserializerNumberTestBase<short>
    {
        public DataReaderDeserializerInt16Test() : base("SELECT CAST({0} AS SMALLINT)", short.MinValue, short.MaxValue)
        {
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMaxValueToSByteThrowsOverflowException()
        {
            DbAssert.AreEqual<sbyte>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMaxValueToByteThrowsOverflowException()
        {
            DbAssert.AreEqual<byte>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMinValueToUInt16ThrowsOverflowException()
        {
            DbAssert.AreEqual<ushort>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MinValue));
        }
    }
}