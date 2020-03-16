using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DbMap.Benchmark
{
    public class Large
    {
        public bool Boolean { get; set; }

        public byte Byte { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Decimal { get; set; }

        public double Double { get; set; }

        public Guid Guid { get; set; }

        public short Int16 { get; set; }

        public int Int32 { get; set; }

        public long Int64 { get; set; }

        public float Single { get; set; }

        [Required]
        public string String { get; set; }

        public bool? NullableBoolean { get; set; }

        public byte? NullableByte { get; set; }

        public DateTime? NullableDateTime { get; set; }

        public decimal? NullableDecimal { get; set; }

        public double? NullableDouble { get; set; }

        public Guid? NullableGuid { get; set; }

        public short? NullableInt16 { get; set; }

        public int? NullableInt32 { get; set; }

        public long? NullableInt64 { get; set; }

        public float? NullableSingle { get; set; }

        public string NullableString { get; set; }

        public static Large Create(int index)
        {
            var offset = index * GetAllPropertyNames().Length;

            return new Large
            {
                Boolean = index % 2 == 0,
                Byte = (byte)((offset + 1) % byte.MaxValue),
                DateTime = new DateTime(2000, 01, 01).AddDays(offset + 2),
                Decimal = offset + 3,
                Double = offset + 4,
                Guid = Guid.Parse("964d9f40-e409-4362-aff4-" + (offset + 5).ToString("x12")),
                Int16 = (short)((offset + 6) % short.MaxValue),
                Int32 = offset + 7,
                Int64 = offset + 8,
                Single = offset + 9,
                String = (offset + 10).ToString(),
                NullableBoolean = index % 2 == 0 ? (bool?)true : null,
                NullableByte = index % 2 == 0 ? (byte?)((offset + 12) % byte.MaxValue) : null,
                NullableDateTime = index % 2 == 0 ? (DateTime?)new DateTime(2000, 01, 01).AddDays(offset + 13) : null,
                NullableDecimal = index % 2 == 0 ? (decimal?)offset + 14 : null,
                NullableDouble = index % 2 == 0 ? (double?)offset + 15 : null,
                NullableGuid = index % 2 == 0 ? (Guid?)Guid.Parse("964d9f40-e409-4362-aff4-" + (offset + 16).ToString("x12")) : null,
                NullableInt16 = index % 2 == 0 ? (short?)((offset + 17) % short.MaxValue) : null,
                NullableInt32 = index % 2 == 0 ? (int?)offset + 18 : null,
                NullableInt64 = index % 2 == 0 ? (int?)offset + 19 : null,
                NullableSingle = index % 2 == 0 ? (int?)offset + 20 : null,
                NullableString = index % 2 == 0 ? (offset + 21).ToString() : null
            };
        }

        public static string[] GetAllPropertyNames() => typeof(Large).GetProperties().Select(property => property.Name).ToArray();

        public override bool Equals(object obj)
        {
            return this.Equals((Large)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Boolean.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte.GetHashCode();
                hashCode = (hashCode * 397) ^ DateTime.GetHashCode();
                hashCode = (hashCode * 397) ^ Decimal.GetHashCode();
                hashCode = (hashCode * 397) ^ Double.GetHashCode();
                hashCode = (hashCode * 397) ^ Guid.GetHashCode();
                hashCode = (hashCode * 397) ^ Int16.GetHashCode();
                hashCode = (hashCode * 397) ^ Int32;
                hashCode = (hashCode * 397) ^ Int64.GetHashCode();
                hashCode = (hashCode * 397) ^ Single.GetHashCode();
                hashCode = (hashCode * 397) ^ (String != null ? String.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ NullableBoolean.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableByte.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableDateTime.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableDecimal.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableDouble.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableGuid.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableInt16.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableInt32.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableInt64.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableSingle.GetHashCode();
                hashCode = (hashCode * 397) ^ (NullableString != null ? NullableString.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(Large other)
        {
            return Boolean == other.Boolean && Byte == other.Byte && DateTime.Equals(other.DateTime) && Decimal == other.Decimal && Double.Equals(other.Double) && Guid.Equals(other.Guid) && Int16 == other.Int16 && Int32 == other.Int32 && Int64 == other.Int64 && Single.Equals(other.Single) && String == other.String && NullableBoolean == other.NullableBoolean && NullableByte == other.NullableByte && Nullable.Equals(NullableDateTime, other.NullableDateTime) && NullableDecimal == other.NullableDecimal && Nullable.Equals(NullableDouble, other.NullableDouble) && Nullable.Equals(NullableGuid, other.NullableGuid) && NullableInt16 == other.NullableInt16 && NullableInt32 == other.NullableInt32 && NullableInt64 == other.NullableInt64 && Nullable.Equals(NullableSingle, other.NullableSingle) && NullableString == other.NullableString;
        }
    }
}