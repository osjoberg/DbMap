using System;
using System.Collections.Generic;

namespace DbMap.Deserialization
{
    internal class DynamicTypeCache
    {
        private static readonly CacheItemComparer CacheItemComparerInstance = new CacheItemComparer();
        private static readonly object CacheLock = new object();
        private static Dictionary<CacheItem, Type> cache = new Dictionary<CacheItem, Type>(CacheItemComparerInstance);

        internal static Type GetCachedOrBuildNew(string[] columnNames, Type[] columnTypes)
        {
            var item = new CacheItem(columnNames, columnTypes);
            if (cache.TryGetValue(item, out var value))
            {
                return value;
            }

            lock (CacheLock)
            {
                if (cache.TryGetValue(item, out value))
                {
                    return value;
                }

                value = DynamicTypeFactory.Create(columnNames, columnTypes);
                cache = new Dictionary<CacheItem, Type>(cache, CacheItemComparerInstance) { { item, value } };
                return value;
            }
        }

        private struct CacheItem
        {
            public CacheItem(string[] columnNames, Type[] columnTypes)
            {
                ColumnNames = columnNames;
                ColumnTypes = columnTypes;
            }

            public string[] ColumnNames { get; }

            public Type[] ColumnTypes { get; }
        }

        private class CacheItemComparer : IEqualityComparer<CacheItem>
        {
            public bool Equals(CacheItem x, CacheItem y)
            {
                if (x.ColumnNames.Length != y.ColumnNames.Length)
                {
                    return false;
                }

                for (var i = 0; i < x.ColumnNames.Length; i++)
                {
                    if (x.ColumnNames[i] != y.ColumnNames[i])
                    {
                        return false;
                    }
                }

                for (var i = 0; i < x.ColumnTypes.Length; i++)
                {
                    if (ReferenceEquals(x.ColumnTypes[i], y.ColumnTypes[i]) == false)
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(CacheItem obj)
            {
                unchecked
                {
                    return obj.ColumnNames.Length ^ obj.ColumnNames[0].GetHashCode() ^ obj.ColumnNames[obj.ColumnNames.Length - 1].GetHashCode();
                }
            }
        }
    }
}
