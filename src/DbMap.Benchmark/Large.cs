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
            if (obj is Large large)
            {
                return
                    Boolean == large.Boolean &&
                    Byte == large.Byte &&
                    DateTime.Equals(large.DateTime) &&
                    Decimal == large.Decimal &&
                    Double == large.Double &&
                    Guid == large.Guid &&
                    Int16 == large.Int16 &&
                    Int32 == large.Int32 &&
                    Int64 == large.Int64 &&
                    Single == large.Single &&
                    String == large.String &&
                    NullableBoolean == large.NullableBoolean &&
                    NullableByte == large.NullableByte &&
                    NullableDateTime == large.NullableDateTime &&
                    NullableDecimal == large.NullableDecimal &&
                    NullableDouble == large.NullableDouble &&
                    NullableGuid == large.NullableGuid &&
                    NullableInt16 == large.NullableInt16 &&
                    NullableInt32 == large.NullableInt32 &&
                    NullableInt64 == large.NullableInt64 &&
                    NullableSingle == large.NullableSingle &&
                    NullableString == large.NullableString;
            }

            var dynamic = (dynamic)obj;

            return
                Boolean == (bool)dynamic.Boolean &&
                Byte == (byte)dynamic.Byte &&
                DateTime == (DateTime)dynamic.DateTime &&
                Decimal == (decimal)dynamic.Decimal &&
                Double == (double)dynamic.Double &&
                Guid == (Guid)dynamic.Guid &&
                Int16 == (short)dynamic.Int16 &&
                Int32 == (int)dynamic.Int32 &&
                Int64 == (long)dynamic.Int64 &&
                Single == (float)dynamic.Single &&
                String == (string)dynamic.String &&
                NullableBoolean == (bool?)dynamic.NullableBoolean &&
                NullableByte == (byte?)dynamic.NullableByte &&
                NullableDateTime == (DateTime?)dynamic.NullableDateTime &&
                NullableDecimal == (decimal?)dynamic.NullableDecimal &&
                NullableDouble == (double?)dynamic.NullableDouble &&
                NullableGuid == (Guid?)dynamic.NullableGuid &&
                NullableInt16 == (short?)dynamic.NullableInt16 &&
                NullableInt32 == (int?)dynamic.NullableInt32 &&
                NullableInt64 == (long?)dynamic.NullableInt64 &&
                NullableSingle == (float?)dynamic.NullableSingle &&
                NullableString == (string)dynamic.NullableString;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}