using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerInt16Test : ParameterSerializerTestBase<short>
    {
        public ParameterSerializerInt16Test() : base("SELECT CAST(IIF(@parameter = {0}, 1, 0) AS BIT)", short.MinValue, short.MaxValue)
        {
        }
    }
}