using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    [TestClass]
    public class DataReaderDeserializerDateTimeTest : DataReaderDeserializerTestBase<DateTime>
    {
        public DataReaderDeserializerDateTimeTest() : base("SELECT CAST(IIF('{0}' = 'NULL', NULL, '{0:yyyy-MM-dd HH:mm:ss.fffffff}') AS DATETIME2)", DateTime.MinValue, DateTime.MaxValue)
        {
        }
    }
}