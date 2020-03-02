using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerByteTest : ParameterSerializerTestBase<byte>
    {
        public ParameterSerializerByteTest() : base("SELECT CAST(IIF(@parameter = {0}, 1, 0) AS BIT)", byte.MinValue, byte.MaxValue)
        {
        }
    }
}