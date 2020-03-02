using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerBooleanTest : DataReaderDeserializerTestBase<bool>
    {
        public DataReaderDeserializerBooleanTest() : base("SELECT CAST(IIF('{0}' = 'NULL', NULL, '{0}') AS BIT)", false, true)
        {
        }
    }
}