using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DbMap.Benchmark
{
    public class Medium
    {
        public bool Boolean { get; set; }

        public decimal Decimal { get; set; }

        public double Double { get; set; }

        public int Int32 { get; set; }

        [Required]
        public string String { get; set; }

        public bool? NullableBoolean { get; set; }

        public decimal? NullableDecimal { get; set; }

        public double? NullableDouble { get; set; }

        public int? NullableInt32 { get; set; }

        public string NullableString { get; set; }

        public static Medium Create(int index)
        {
            var offset = index * GetAllPropertyNames().Length;

            return new Medium
            {
                Boolean = index % 2 == 0,
                Decimal = offset + 1,
                Double = offset + 2,
                Int32 = offset + 3,
                String = (offset + 4).ToString(),
                NullableBoolean = index % 2 == 0 ? (bool?)true : null,
                NullableDecimal = index % 2 == 0 ? (decimal?)offset + 6 : null,
                NullableDouble = index % 2 == 0 ? (double?)offset + 7 : null,
                NullableInt32 = index % 2 == 0 ? (int?)offset + 8 : null,
                NullableString = index % 2 == 0 ? (offset + 9).ToString() : null
            };
        }

        public static string[] GetAllPropertyNames() => typeof(Medium).GetProperties().Select(property => property.Name).ToArray();

        public override bool Equals(object obj)
        {
            return this.Equals((Medium)obj);
        }

        protected bool Equals(Medium other)
        {
            return Boolean == other.Boolean && Decimal == other.Decimal && Double.Equals(other.Double) && Int32 == other.Int32 && String == other.String && NullableBoolean == other.NullableBoolean && NullableDecimal == other.NullableDecimal && Nullable.Equals(NullableDouble, other.NullableDouble) && NullableInt32 == other.NullableInt32 && NullableString == other.NullableString;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Boolean.GetHashCode();
                hashCode = (hashCode * 397) ^ Decimal.GetHashCode();
                hashCode = (hashCode * 397) ^ Double.GetHashCode();
                hashCode = (hashCode * 397) ^ Int32;
                hashCode = (hashCode * 397) ^ (String != null ? String.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ NullableBoolean.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableDecimal.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableDouble.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableInt32.GetHashCode();
                hashCode = (hashCode * 397) ^ (NullableString != null ? NullableString.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}