using System;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerInt32Test : DataReaderDeserializerNumberTestBase<int>
    {
        public DataReaderDeserializerInt32Test() : base("SELECT {0}", int.MinValue, int.MaxValue)
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
        public virtual void DeserializeMinValueToUInt32ThrowsOverflowException()
        {
            DbAssert.AreEqual<uint>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MinValue));
        }
    }
}