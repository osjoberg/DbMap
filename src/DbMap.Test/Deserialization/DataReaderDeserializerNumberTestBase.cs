using System;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    public abstract class DataReaderDeserializerNumberTestBase<TReturn> : DataReaderDeserializerTestBase<TReturn> where TReturn : struct
    {
        protected DataReaderDeserializerNumberTestBase(string queryFormat, TReturn min, TReturn max) : base(queryFormat, min, max)
        {
        }

        [TestMethod]
        public virtual void CanDeserializeValueToSByte()
        {
            DbAssert.AreEqual<sbyte>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToByte()
        {
            DbAssert.AreEqual<byte>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToChar()
        {
            DbAssert.AreEqual<char>((char)127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToInt16()
        {
            DbAssert.AreEqual<short>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToUInt16()
        {
            DbAssert.AreEqual<ushort>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToInt32()
        {
            DbAssert.AreEqual<int>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToUInt32()
        {
            DbAssert.AreEqual<uint>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToInt64()
        {
            DbAssert.AreEqual<long>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToUInt64()
        {
            DbAssert.AreEqual<ulong>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToSingle()
        {
            DbAssert.AreEqual<float>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToDouble()
        {
            DbAssert.AreEqual<double>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToDecimal()
        {
            DbAssert.AreEqual<decimal>(127, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeZeroValueToFalseBoolean()
        {
            DbAssert.AreEqual<bool>(false, string.Format(CultureInfo.InvariantCulture, QueryFormat, 0));
        }

        [TestMethod]
        public virtual void CanDeserializeNonZeroValueToTrueBoolean()
        {
            DbAssert.AreEqual<bool>(true, string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }

        [TestMethod]
        public virtual void CanDeserializeValueToObject()
        {
            DbAssert.AreEqual<object>(Convert.ChangeType(127, typeof(TReturn)), string.Format(CultureInfo.InvariantCulture, QueryFormat, 127));
        }
    }
}
