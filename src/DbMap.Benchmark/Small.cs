using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DbMap.Benchmark
{
    public class Small
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

        public static string[] GetAllPropertyNames() => typeof(Small).GetProperties().Select(property => property.Name).ToArray();

        public override bool Equals(object obj)
        {
            return this.Equals((Small)obj);
        }

        protected bool Equals(Small other)
        {
            return Boolean == other.Boolean && Int32 == other.Int32 && String == other.String && NullableBoolean == other.NullableBoolean && NullableInt32 == other.NullableInt32 && NullableString == other.NullableString;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Boolean.GetHashCode();
                hashCode = (hashCode * 397) ^ Int32;
                hashCode = (hashCode * 397) ^ (String != null ? String.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ NullableBoolean.GetHashCode();
                hashCode = (hashCode * 397) ^ NullableInt32.GetHashCode();
                hashCode = (hashCode * 397) ^ (NullableString != null ? NullableString.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}