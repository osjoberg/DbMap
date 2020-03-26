using System;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerSingleTest : DataReaderDeserializerNumberTestBase<float>
    {
        public DataReaderDeserializerSingleTest() : base("SELECT CAST({0} AS REAL)", float.MinValue, float.MaxValue)
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
        public void DeserializeMaxValueToInt16ThrowsOverflowException()
        {
            DbAssert.AreEqual<short>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMaxValueToUInt16ThrowsOverflowException()
        {
            DbAssert.AreEqual<ushort>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMaxValueToCharThrowsOverflowException()
        {
            DbAssert.AreEqual<char>('\0', string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMaxValueToInt32ThrowsOverflowException()
        {
            DbAssert.AreEqual<int>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMaxValueToUInt32ThrowsOverflowException()
        {
            DbAssert.AreEqual<uint>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMaxValueToInt64ThrowsOverflowException()
        {
            DbAssert.AreEqual<long>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMaxValueToUInt64ThrowsOverflowException()
        {
            DbAssert.AreEqual<ulong>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DeserializeMaxValueToDecimalThrowsOverflowException()
        {
            DbAssert.AreEqual<decimal>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }
    }
}