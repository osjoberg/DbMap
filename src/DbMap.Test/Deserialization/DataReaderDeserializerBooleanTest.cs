using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerBooleanTest : DataReaderDeserializerNumberTestBase<bool>
    {
        public DataReaderDeserializerBooleanTest() : base("SELECT CAST(IIF('{0}' = 'NULL', NULL, '{0}') AS BIT)", false, true)
        {
        }

        [TestMethod]
        public override void CanDeserializeValueToSByte()
        {
            DbAssert.AreEqual<sbyte>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToByte()
        {
            DbAssert.AreEqual<byte>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToChar()
        {
            DbAssert.AreEqual<char>((char)1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToInt16()
        {
            DbAssert.AreEqual<short>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToUInt16()
        {
            DbAssert.AreEqual<ushort>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToInt32()
        {
            DbAssert.AreEqual<int>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToUInt32()
        {
            DbAssert.AreEqual<uint>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToInt64()
        {
            DbAssert.AreEqual<long>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToUInt64()
        {
            DbAssert.AreEqual<ulong>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToSingle()
        {
            DbAssert.AreEqual<float>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToDouble()
        {
            DbAssert.AreEqual<double>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeValueToDecimal()
        {
            DbAssert.AreEqual<decimal>(1, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public override void CanDeserializeZeroValueToFalseBoolean()
        {
            DbAssert.AreEqual<bool>(false, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public override void CanDeserializeNonZeroValueToTrueBoolean()
        {
            DbAssert.AreEqual<bool>(true, string.Format(CultureInfo.InvariantCulture, QueryFormat, true));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToSByte()
        {
            DbAssert.AreEqual<sbyte>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToByte()
        {
            DbAssert.AreEqual<byte>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToChar()
        {
            DbAssert.AreEqual<char>((char)0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToInt06()
        {
            DbAssert.AreEqual<short>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToUInt06()
        {
            DbAssert.AreEqual<ushort>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToInt32()
        {
            DbAssert.AreEqual<int>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToUInt32()
        {
            DbAssert.AreEqual<uint>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToInt64()
        {
            DbAssert.AreEqual<long>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToUInt64()
        {
            DbAssert.AreEqual<ulong>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToSingle()
        {
            DbAssert.AreEqual<float>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToDouble()
        {
            DbAssert.AreEqual<double>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }

        [TestMethod]
        public void CanDeserializeZeroValueToDecimal()
        {
            DbAssert.AreEqual<decimal>(0, string.Format(CultureInfo.InvariantCulture, QueryFormat, false));
        }
    }
}