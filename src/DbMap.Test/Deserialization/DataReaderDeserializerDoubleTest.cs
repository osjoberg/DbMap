using System;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerDoubleTest : DataReaderDeserializerNumberTestBase<double>
    {
        public DataReaderDeserializerDoubleTest() : base("SELECT CAST({0} AS FLOAT)", double.MinValue, double.MaxValue)
        {
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToSByteThrowsOverflowException()
        {
            DbAssert.AreEqual<sbyte>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToByteThrowsOverflowException()
        {
            DbAssert.AreEqual<byte>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToInt16ThrowsOverflowException()
        {
            DbAssert.AreEqual<short>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToUInt16ThrowsOverflowException()
        {
            DbAssert.AreEqual<ushort>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToCharThrowsOverflowException()
        {
            DbAssert.AreEqual<char>('\0', string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToInt32ThrowsOverflowException()
        {
            DbAssert.AreEqual<int>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToUInt32ThrowsOverflowException()
        {
            DbAssert.AreEqual<uint>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToInt64ThrowsOverflowException()
        {
            DbAssert.AreEqual<long>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToUInt64ThrowsOverflowException()
        {
            DbAssert.AreEqual<ulong>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToDecimalThrowsOverflowException()
        {
            DbAssert.AreEqual<decimal>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        public virtual void DeserializeMaxValueToSingleResultsInPositiveInfinity()
        {
            DbAssert.AreEqual<float>(float.PositiveInfinity, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }
    }
}