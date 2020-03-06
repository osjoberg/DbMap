using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerInitializationTest
    {
        [TestMethod]
        public void CanDeserializeInitializedString()
        {
            var expected = new InitializedString
            {
                Value1 = null,
                Value2 = "New value 2",
                Value3 = "Initialized value 3",
            };

            DbAssert.AreEqual(expected, "SELECT NULL AS Value1, 'New value 2' AS Value2", false);
        }

        [TestMethod]
        public void CanDeserializeInitializedNullableInt()
        {
            var expected = new InitializedInt32
            {
                Value1 = null,
                Value2 = 2222,
                Value3 = 3,
            };

            DbAssert.AreEqual(expected, "SELECT NULL AS Value1, 2222 AS Value2", false);
        }

        internal class InitializedString
        {
            public string Value1 { get; set; } = "Initialized value 1";

            public string Value2 { get; set; } = "Initialized value 2";

            public string Value3 { get; set; } = "Initialized value 3";

            public override bool Equals(object obj)
            {
                var other = (InitializedString)obj;
                return Value1 == other.Value1 && Value2 == other.Value2 && Value3 == other.Value3;
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }
        }

        internal class InitializedInt32
        {
            public int? Value1 { get; set; } = 1;

            public int? Value2 { get; set; } = 2;

            public int? Value3 { get; set; } = 3;

            public override bool Equals(object obj)
            {
                var other = (InitializedInt32)obj;
                return Value1 == other.Value1 && Value2 == other.Value2 && Value3 == other.Value3;
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }
        }
    }
}
