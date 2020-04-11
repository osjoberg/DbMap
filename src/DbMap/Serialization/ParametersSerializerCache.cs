using System;
using System.Collections.Generic;
 
namespace DbMap.Serialization
{
    internal static class ParametersSerializerCache
    {
        private static readonly CacheItemComparer Comparer = new CacheItemComparer();
        private static readonly object CacheLock = new object();
        private static Dictionary<CacheItem, ParametersSerializer> cache = new Dictionary<CacheItem, ParametersSerializer>(Comparer);

        public static ParametersSerializer GetCachedOrBuildNew(Type connectionType, Type parametersType)
        {
            var cacheItem = new CacheItem(parametersType, connectionType);

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

                value = ParametersSerializerFactory.Create(connectionType, parametersType);
                cache = new Dictionary<CacheItem, ParametersSerializer>(cache, Comparer) { { cacheItem, value } };
                return value;
            }
        }

        private struct CacheItem
        {
            public CacheItem(Type parametersType, Type connectionType)
            {
                ConnectionType = connectionType;
                ParametersType = parametersType;
            }

            public Type ParametersType { get; }

            public Type ConnectionType { get; }
        }

        private class CacheItemComparer : IEqualityComparer<CacheItem>
        {
            public bool Equals(CacheItem x, CacheItem y)
            {
                return ReferenceEquals(x.ParametersType, y.ParametersType) && ReferenceEquals(x.ConnectionType, y.ConnectionType);
            }

            public int GetHashCode(CacheItem obj)
            {
                return obj.ParametersType.GetHashCode() ^ obj.ConnectionType.GetHashCode();
            }
        }
    }
}
