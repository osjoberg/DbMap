using System;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerByteEnumTest : DataReaderDeserializerNumberTestBase<DataReaderDeserializerByteEnumTest.ByteEnum>
    {
        public DataReaderDeserializerByteEnumTest() : base("SELECT CAST({0:d} AS TINYINT)", ByteEnum.MinValue, ByteEnum.MaxValue)
        {
        }

        public enum ByteEnum : byte
        {
            MinValue = byte.MinValue,
            MaxValue = byte.MaxValue
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public virtual void DeserializeMaxValueToSByteThrowsOverflowException()
        {
            DbAssert.AreEqual<sbyte>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }
    }
}