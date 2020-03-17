using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DbMap.Benchmark
{
    public class ExtraSmall
    {
        public bool Boolean { get; set; }

        public int Int32 { get; set; }

        [Required]
        public string String { get; set; }

        public bool? NullableBoolean { get; set; }

        public int? NullableInt32 { get; set; }

        public string NullableString { get; set; }

        public static Small Create(int index)
        {
            var offset = index * GetAllPropertyNames().Length;

            return new Small
            {
                Boolean = index % 2 == 0,
                Int32 = offset + 1,
                String = (offset + 2).ToString(),
                NullableBoolean = index % 2 == 0 ? (bool?)true : null,
                NullableInt32 = index % 2 == 0 ? (int?)offset + 4 : null,
                NullableString = index % 2 == 0 ? (offset + 5).ToString() : null
            };
        }

        public static string[] GetAllPropertyNames() => typeof(ExtraSmall).GetProperties().Select(property => property.Name).ToArray();

        public override bool Equals(object obj)
        {
            return Equals((ExtraSmall)obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        protected bool Equals(ExtraSmall other)
        {
            return Boolean == other.Boolean && Int32 == other.Int32 && String == other.String && NullableBoolean == other.NullableBoolean && NullableInt32 == other.NullableInt32 && NullableString == other.NullableString;
        }
    }
}