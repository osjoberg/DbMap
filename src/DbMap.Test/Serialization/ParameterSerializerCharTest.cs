using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerCharTest : ParameterSerializerTestBase<char>
    {
        public ParameterSerializerCharTest() : base("SELECT CAST(IIF((@parameter IS NULL AND '{0}' = 'NULL') OR @parameter = N'{0}', 1, 0) AS BIT)", char.MinValue, char.MaxValue) 
        {
        }
    }
}