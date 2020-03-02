using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    public abstract class ParameterSerializerTestBase<TParameter> where TParameter : struct
    {
        private readonly TParameter min;
        private readonly TParameter max;
        private readonly string queryFormat;

        protected ParameterSerializerTestBase(string queryFormat, TParameter min, TParameter max)
        {
            this.queryFormat = queryFormat;
            this.min = min;
            this.max = max;
        }

        [TestMethod]
        public virtual void CanSerializeMaxValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, queryFormat, max), new { parameter = max });
        }

        [TestMethod]
        public virtual void CanSerializeDefaultValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, queryFormat, default(TParameter)), new { parameter = default(TParameter) });
        }

        [TestMethod]
        public virtual void CanSerializeMinValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, queryFormat, min), new { parameter = min });
        }

        [TestMethod]
        public virtual void CanSerializeNullableMaxValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, queryFormat, (TParameter?)max), new { parameter = (TParameter?)max });
        }

        [TestMethod]
        public virtual void CanSerializeNullableDefaultValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, queryFormat, (TParameter?)default(TParameter)), new { parameter = (TParameter?)default(TParameter) });
        }

        [TestMethod]
        public virtual void CanSerializeNullableMinValue()
        {
            DbAssert.IsTrue(string.Format(CultureInfo.InvariantCulture, queryFormat, (TParameter?)min), new { parameter = (TParameter?)min });
        }

        [TestMethod]
        public virtual void CanSerializeNullableNullValue()
        {
            DbAssert.IsTrue("SELECT CAST(IIF(@parameter IS NULL, 1, 0) AS BIT)", new { parameter = (TParameter?)null });
        }
    }
}
