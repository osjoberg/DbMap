using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerCharTest : DataReaderDeserializerTestBase<char>
    { 
        public DataReaderDeserializerCharTest() : base("SELECT CAST(IIF('{0}' = 'NULL', NULL, '{0}') AS NVARCHAR)", char.MinValue, char.MaxValue)
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
    }
}