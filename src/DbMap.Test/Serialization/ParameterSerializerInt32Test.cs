using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerInt32Test : ParameterSerializerTestBase<int>
    {
        public ParameterSerializerInt32Test() : base("SELECT CAST(IIF(@parameter = {0}, 1, 0) AS BIT)", int.MinValue, int.MaxValue)
        {
        }
    }
}