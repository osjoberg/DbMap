using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerCharTest : DataReaderDeserializerTestBase<char>
    { 
        public DataReaderDeserializerCharTest() : base("SELECT IIF('{0}' = 'NULL', NULL, '{0}')", char.MinValue, char.MaxValue)
        {
        }


        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void CanDeserializeMaxValue()
        {
            base.CanDeserializeMaxValue();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void CanDeserializeDefaultValue()
        {
            base.CanDeserializeDefaultValue();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void CanDeserializeMinValue()
        {
            base.CanDeserializeMinValue();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void CanDeserializeNullableMaxValue()
        {
            base.CanDeserializeNullableMaxValue();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void CanDeserializeNullableDefaultValue()
        {
            base.CanDeserializeNullableDefaultValue();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void CanDeserializeNullableMinValue()
        {
            base.CanDeserializeNullableMinValue();
        }
    }
}