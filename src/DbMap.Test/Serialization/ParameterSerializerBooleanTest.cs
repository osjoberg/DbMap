using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterDeserializerBooleanTest : ParameterSerializerTestBase<bool>
    {
        public ParameterDeserializerBooleanTest() : base("SELECT CAST(IIF((@parameter = 1 AND '{0}' = 'True') OR (@parameter = 0 AND '{0}' = 'False'), 1, 0) AS BIT)", false, true)
        {
        }
    }
}