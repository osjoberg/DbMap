using System;
using System.Collections.Generic;

namespace DbMap.Deserialization
{
    internal static class ScalarDeserializerCache
    {
        private static readonly TypeComparer Comparer = new TypeComparer();
        private static readonly object CacheLock = new object();
        private static Dictionary<Type, ScalarDeserializer> cache = new Dictionary<Type, ScalarDeserializer>(Comparer);

        public static ScalarDeserializer GetCachedOrBuildNew(Type type)
        {
            if (cache.TryGetValue(type, out var value))
            {
                return value;
            }

            lock (CacheLock)
            {
                if (cache.TryGetValue(type, out value))
                {
                    return value;
                }

                value = ScalarDeserializer.Create(type);
                cache = new Dictionary<Type, ScalarDeserializer>(cache, Comparer) { { type, value } };
                return value;
            }
        }

        private class TypeComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                return ReferenceEquals(x, y);
            }

            public int GetHashCode(Type obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
