using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerInt32EnumTest : ParameterSerializerTestBase<ParameterSerializerInt32EnumTest.Int32Enum>
    {
        public ParameterSerializerInt32EnumTest() : base("SELECT CAST(IIF(@parameter = {0:d}, 1, 0) AS BIT)", Int32Enum.MinValue, Int32Enum.MaxValue)
        {
        }

        public enum Int32Enum
        {
            MinValue = int.MinValue,
            MaxValue = int.MaxValue
        }
    }
}