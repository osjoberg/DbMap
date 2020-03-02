using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    public abstract class DataReaderDeserializerTestBase<TReturn> where TReturn : struct
    {
        private readonly TReturn min;
        private readonly TReturn max;
        private readonly string queryFormat;

        protected DataReaderDeserializerTestBase(string queryFormat, TReturn min, TReturn max)
        {
            this.queryFormat = queryFormat;
            this.min = min;
            this.max = max;
        }

        [TestMethod]
        public virtual void CanDeserializeMaxValue()
        {
            DbAssert.AreEqual(max, string.Format(CultureInfo.InvariantCulture, queryFormat, max));
        }

        [TestMethod]
        public virtual void CanDeserializeDefaultValue()
        {
            DbAssert.AreEqual<TReturn>(default, string.Format(CultureInfo.InvariantCulture, queryFormat, default(TReturn)));
        }

        [TestMethod]
        public virtual void CanDeserializeMinValue()
        {
            DbAssert.AreEqual(min, string.Format(CultureInfo.InvariantCulture, queryFormat, min));
        }

        [TestMethod]
        public virtual void CanDeserializeNullableMaxValue()
        {
            DbAssert.AreEqual((TReturn?)max, string.Format(CultureInfo.InvariantCulture, queryFormat, max));
        }

        [TestMethod]
        public virtual void CanDeserializeNullableDefaultValue()
        {
            DbAssert.AreEqual((TReturn?)default(TReturn), string.Format(CultureInfo.InvariantCulture, queryFormat, default(TReturn)));
        }

        [TestMethod]
        public virtual void CanDeserializeNullableMinValue()
        {
            DbAssert.AreEqual((TReturn?)min, string.Format(CultureInfo.InvariantCulture, queryFormat, min));
        }

        [TestMethod]
        public void CanDeserializeNullableNullValue()
        {
            DbAssert.AreEqual<TReturn>(null, string.Format(CultureInfo.InvariantCulture, queryFormat, "NULL"));
        }
    }
}
