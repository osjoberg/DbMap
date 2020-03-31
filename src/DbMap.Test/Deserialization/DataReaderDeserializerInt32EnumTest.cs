using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerInt32EnumTest : DataReaderDeserializerTestBase<DataReaderDeserializerInt32EnumTest.Int32Enum>
    {
        public DataReaderDeserializerInt32EnumTest() : base("SELECT {0:d}", Int32Enum.MinValue, Int32Enum.MaxValue)
        {
        }

        public enum Int32Enum
        {
            MinValue = int.MinValue,
            MaxValue = int.MaxValue
        }
    }
}