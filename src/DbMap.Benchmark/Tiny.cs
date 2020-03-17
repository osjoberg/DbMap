using System.Linq;

namespace DbMap.Benchmark
{
    public class Tiny
    {
        public string String { get; set; }

        public static Tiny Create(int index)
        {
            var offset = index * GetAllPropertyNames().Length;

            return new Tiny
            {
                String = (offset + 2).ToString(),
            };
        }

        public static string[] GetAllPropertyNames() => typeof(Tiny).GetProperties().Select(property => property.Name).ToArray();
    }
}