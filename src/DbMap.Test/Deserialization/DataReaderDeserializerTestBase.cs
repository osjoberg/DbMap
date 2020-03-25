using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Deserialization
{
    public abstract class DataReaderDeserializerTestBase<TReturn> where TReturn : struct
    {
        protected readonly string QueryFormat;
        protected readonly TReturn MinValue;
        protected readonly TReturn MaxValue;

        protected DataReaderDeserializerTestBase(string queryFormat, TReturn minValue, TReturn maxValue)
        {
            QueryFormat = queryFormat;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        [TestMethod]
        public virtual void CanDeserializeMaxValue()
        {
            DbAssert.AreEqual(MaxValue, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        public virtual void CanDeserializeDefaultValue()
        {
            DbAssert.AreEqual<TReturn>(default, string.Format(CultureInfo.InvariantCulture, QueryFormat, default(TReturn)));
        }

        [TestMethod]
        public virtual void CanDeserializeMinValue()
        {
            DbAssert.AreEqual(MinValue, string.Format(CultureInfo.InvariantCulture, QueryFormat, MinValue));
        }

        [TestMethod]
        public virtual void CanDeserializeNullableMaxValue()
        {
            DbAssert.AreEqual((TReturn?)MaxValue, string.Format(CultureInfo.InvariantCulture, QueryFormat, MaxValue));
        }

        [TestMethod]
        public virtual void CanDeserializeNullableDefaultValue()
        {
            DbAssert.AreEqual((TReturn?)default(TReturn), string.Format(CultureInfo.InvariantCulture, QueryFormat, default(TReturn)));
        }

        [TestMethod]
        public virtual void CanDeserializeNullableMinValue()
        {
            DbAssert.AreEqual((TReturn?)MinValue, string.Format(CultureInfo.InvariantCulture, QueryFormat, MinValue));
        }

        [TestMethod]
        public virtual void CanDeserializeNullableNullValue()
        {
            DbAssert.AreEqual<TReturn>(null, string.Format(CultureInfo.InvariantCulture, QueryFormat, "NULL"));
        }
    }
}
