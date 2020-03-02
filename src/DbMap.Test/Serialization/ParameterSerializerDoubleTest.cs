using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerDoubleTest : ParameterSerializerTestBase<double>
    {
        public ParameterSerializerDoubleTest() : base("SELECT CAST(IIF(@parameter = CAST({0} AS FLOAT), 1, 0) AS BIT)", double.MinValue, double.MaxValue)
        {
        }
    }
}