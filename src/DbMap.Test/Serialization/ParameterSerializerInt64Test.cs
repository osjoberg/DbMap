using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerInt64Test : ParameterSerializerTestBase<long>
    {
        public ParameterSerializerInt64Test() : base("SELECT CAST(IIF(@parameter = {0}, 1, 0) AS BIT)", long.MinValue, long.MaxValue)
        {
        }
    }
}