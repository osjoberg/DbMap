using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerGuidTest : DataReaderDeserializerTestBase<Guid>
    { 
        public DataReaderDeserializerGuidTest() : base("SELECT CAST(IIF('{0}' = 'NULL', NULL, '{0}') AS UNIQUEIDENTIFIER)", Guid.Empty, Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"))
        {
        }
    }
}