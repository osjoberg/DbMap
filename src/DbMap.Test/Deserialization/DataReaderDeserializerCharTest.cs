using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerCharTest : DataReaderDeserializerNumberTestBase<char>
    { 
        public DataReaderDeserializerCharTest() : base("SELECT CAST(IIF('{0}' = 'NULL', NULL, '{0}') AS NCHAR)", char.MinValue, char.MaxValue)
        {
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeMaxValue()
        {
            base.CanDeserializeMaxValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeDefaultValue()
        {
            base.CanDeserializeDefaultValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeMinValue()
        {
            base.CanDeserializeMinValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeNullableMaxValue()
        {
            base.CanDeserializeNullableMaxValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeNullableDefaultValue()
        {
            base.CanDeserializeNullableDefaultValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeNullableMinValue()
        {
            base.CanDeserializeNullableMinValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeNullableNullValue()
        {
            base.CanDeserializeNullableNullValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToSByte()
        {
            base.CanDeserializeValueToSByte();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToByte()
        {
            base.CanDeserializeValueToByte();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToChar()
        {
            base.CanDeserializeValueToChar();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToInt16()
        {
            base.CanDeserializeValueToInt16();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToUInt16()
        {
            base.CanDeserializeValueToUInt16();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToInt32()
        {
            base.CanDeserializeValueToInt32();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToUInt32()
        {
            base.CanDeserializeValueToUInt32();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToInt64()
        {
            base.CanDeserializeValueToInt64();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToUInt64()
        {
            base.CanDeserializeValueToUInt64();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToSingle()
        {
            base.CanDeserializeValueToSingle();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToDouble()
        {
            base.CanDeserializeValueToDouble();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeValueToDecimal()
        {
            base.CanDeserializeValueToDecimal();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeZeroValueToFalseBoolean()
        {
            base.CanDeserializeZeroValueToFalseBoolean();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public override void CanDeserializeNonZeroValueToTrueBoolean()
        {
            base.CanDeserializeNonZeroValueToTrueBoolean();
        }
    }
}