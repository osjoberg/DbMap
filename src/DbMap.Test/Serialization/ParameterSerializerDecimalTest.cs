using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerDecimalTest : ParameterSerializerTestBase<decimal>
    {
        public ParameterSerializerDecimalTest() : base("SELECT CAST(IIF(@parameter = CAST({0} AS DECIMAL(38,0)), 1, 0) AS BIT)", decimal.MinValue, decimal.MaxValue)
        {
        }
    }
}