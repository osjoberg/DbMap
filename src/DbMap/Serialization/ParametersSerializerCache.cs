using System;
using System.Collections.Generic;

namespace DbMap.Serialization
{
    internal static class ParametersSerializerCache
    {
        private static readonly CacheItemComparer Comparer = new CacheItemComparer();
        private static readonly object CacheLock = new object();
        private static Dictionary<CacheItem, ParametersSerializer> cache = new Dictionary<CacheItem, ParametersSerializer>(Comparer);

        public static ParametersSerializer GetCachedOrBuildNew(Type dataParameterType, Type parametersType)
        {
            var cacheItem = new CacheItem(dataParameterType, parametersType);

            if (cache.TryGetValue(cacheItem, out var value))
            {
                return value;
            }

            lock (CacheLock)
            {
                if (cache.TryGetValue(cacheItem, out value))
                {
                    return value;
                }

                value = ParametersSerializerFactory.Create(dataParameterType, parametersType);
                cache = new Dictionary<CacheItem, ParametersSerializer>(cache, Comparer) { { cacheItem, value } };
                return value;
            }
        }

        private struct CacheItem
        {
            public CacheItem(Type dataParameterType, Type parametersType)
            {
                DataParameterType = dataParameterType;
                ParametersType = parametersType;
            }

            public Type DataParameterType { get; }

            public Type ParametersType { get; }
        }

        private class CacheItemComparer : IEqualityComparer<CacheItem>
        {
            public bool Equals(CacheItem x, CacheItem y)
            {
                return ReferenceEquals(x.ParametersType, y.ParametersType) && ReferenceEquals(x.DataParameterType, y.DataParameterType);
            }

            public int GetHashCode(CacheItem obj)
            {
                return obj.DataParameterType.GetHashCode() ^ obj.ParametersType.GetHashCode();
            }
        }
    }
}
