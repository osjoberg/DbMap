using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerSingleTest : ParameterSerializerTestBase<float>
    {
        public ParameterSerializerSingleTest() : base("SELECT CAST(IIF(@parameter = CAST({0} AS REAL), 1, 0) AS BIT)", float.MinValue, float.MaxValue)
        {
        }
    }
}