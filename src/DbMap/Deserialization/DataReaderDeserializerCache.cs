using System;
using System.Collections.Generic;

namespace DbMap.Deserialization
{
    internal static class DataReaderDeserializerCache
    {
        private static readonly CacheItemComparer CacheItemComparerInstance = new CacheItemComparer();
        private static readonly TypeComparer TypeComparerInstance = new TypeComparer();

        private static readonly object CacheLock = new object();

        private static Dictionary<Type, DataReaderDeserializer> clrCache = new Dictionary<Type, DataReaderDeserializer>(TypeComparerInstance);
        private static Dictionary<CacheItem, DataReaderDeserializer> cache = new Dictionary<CacheItem, DataReaderDeserializer>(CacheItemComparerInstance);

        internal static DataReaderDeserializer GetCachedOrBuildNew(Type type, string[] columnNames)
        {
            if (columnNames == null)
            {
                if (clrCache.TryGetValue(type, out var clrValue))
                {
                    return clrValue;
                }

                lock (CacheLock)
                {
                    if (clrCache.TryGetValue(type, out clrValue))
                    {
                        return clrValue;
                    }

                    clrValue = DataReaderDeserializer.Create(type, null);
                    clrCache = new Dictionary<Type, DataReaderDeserializer>(clrCache, TypeComparerInstance) { { type, clrValue } };
                    return clrValue;
                }
            }

            var item = new CacheItem(type, columnNames);
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

                value = DataReaderDeserializer.Create(type, columnNames);
                cache = new Dictionary<CacheItem, DataReaderDeserializer>(cache, CacheItemComparerInstance) { { item, value } };
                return value;
            }
        }

        private struct CacheItem
        {
            public CacheItem(Type type, string[] columnNames)
            {
                Type = type;
                ColumnNames = columnNames;
            }

            public Type Type { get; }

            public string[] ColumnNames { get; }
        }

        private class CacheItemComparer : IEqualityComparer<CacheItem>
        {
            public bool Equals(CacheItem x, CacheItem y)
            {
                if (ReferenceEquals(x.Type, y.Type) == false)
                {
                    return false;
                }

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

                return true;
            }

            public int GetHashCode(CacheItem obj)
            {
                unchecked
                {
                    return obj.Type.GetHashCode() ^ obj.ColumnNames.Length ^ obj.ColumnNames[0].GetHashCode() ^ obj.ColumnNames[obj.ColumnNames.Length - 1].GetHashCode();
                }
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
