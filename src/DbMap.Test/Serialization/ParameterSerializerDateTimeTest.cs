using System;
using System.Data.SqlTypes;
using System.Globalization;

using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerDateTimeTest : ParameterSerializerTestBase<DateTime>
    {
        public ParameterSerializerDateTimeTest() : base("SELECT CAST(IIF(@parameter = CAST('{0:yyyy-MM-dd HH:mm:ss.fff}' AS DATETIME), 1, 0) AS BIT)", DateTime.MinValue, DateTime.MaxValue)
        {
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public override void CanSerializeMaxValue()
        {
            base.CanSerializeMaxValue();
        }

        [TestMethod]
        [ExpectedException(typeof(SqlTypeException))]
        public override void CanSerializeDefaultValue()
        {
            base.CanSerializeDefaultValue();
        }

        [TestMethod]
        [ExpectedException(typeof(SqlTypeException))]
        public override void CanSerializeMinValue()
        {
            base.CanSerializeMinValue();
        }

        [TestMethod]
        [ExpectedException(typeof(SqlTypeException))]
        public override void CanSerializeNullableDefaultValue()
        {
            base.CanSerializeNullableDefaultValue();
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public override void CanSerializeNullableMaxValue()
        {
            base.CanSerializeNullableMaxValue();
        }

        [TestMethod]
        [ExpectedException(typeof(SqlTypeException))]
        public override void CanSerializeNullableMinValue()
        {
            base.CanSerializeNullableMinValue();
        }

        [TestMethod]
        public void CanSerializeSqlServerMaxValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, "SELECT CAST(IIF(@parameter = CAST('{0:yyyy-MM-dd HH:mm:ss.fff}' AS DATETIME), 1, 0) AS BIT)", SqlDateTime.MaxValue.Value), new { parameter = SqlDateTime.MaxValue.Value });
        }

        [TestMethod]
        public void CanSerializeSqlServerMinValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, "SELECT CAST(IIF(@parameter = CAST('{0:yyyy-MM-dd HH:mm:ss.fff}' AS DATETIME), 1, 0) AS BIT)", SqlDateTime.MinValue.Value), new { parameter = SqlDateTime.MinValue.Value });
        }

        [TestMethod]
        public void CanSerializeNullableSqlServerMaxValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, "SELECT CAST(IIF(@parameter = CAST('{0:yyyy-MM-dd HH:mm:ss.fff}' AS DATETIME), 1, 0) AS BIT)", (DateTime?)SqlDateTime.MaxValue.Value), new { parameter = (DateTime?)SqlDateTime.MaxValue.Value });
        }

        [TestMethod]
        public void CanSerializeNullableSqlServerMinValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, "SELECT CAST(IIF(@parameter = CAST('{0:yyyy-MM-dd HH:mm:ss.fff}' AS DATETIME), 1, 0) AS BIT)", (DateTime?)SqlDateTime.MinValue.Value), new { parameter = (DateTime?)SqlDateTime.MinValue.Value });
        }
    }
}