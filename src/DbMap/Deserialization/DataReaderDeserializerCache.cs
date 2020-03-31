using System;
using System.Collections.Generic;

namespace DbMap.Deserialization
{
    internal static class DataReaderDeserializerCache
    {
        private static readonly CacheItemComparer CacheItemComparerInstance = new CacheItemComparer();
        private static readonly object CacheLock = new object();
        private static Dictionary<CacheItem, DataReaderDeserializer> cache = new Dictionary<CacheItem, DataReaderDeserializer>(CacheItemComparerInstance);

        internal static DataReaderDeserializer GetCachedOrBuildNew(Type dataReaderType, Type type, string[] columnNames, Type[] columnTypes)
        {
            var item = new CacheItem(dataReaderType, type, columnNames, columnTypes);
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

                value = DataReaderDeserializerFactory.Create(dataReaderType, type, columnNames, columnTypes);
                cache = new Dictionary<CacheItem, DataReaderDeserializer>(cache, CacheItemComparerInstance) { { item, value } };
                return value;
            }
        }

        private struct CacheItem
        {
            public CacheItem(Type dataReaderType, Type type, string[] columnNames, Type[] columnTypes)
            {
                DataReaderType = dataReaderType;
                Type = type;
                ColumnNames = columnNames;
                ColumnTypes = columnTypes;
            }

            public Type DataReaderType { get; }

            public Type Type { get; }

            public string[] ColumnNames { get; }

            public Type[] ColumnTypes { get; }
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

                for (var i = 0; i < x.ColumnTypes.Length; i++)
                {
                    if (ReferenceEquals(x.ColumnTypes[i], y.ColumnTypes[i]) == false)
                    {
                        return false;
                    }
                }

                if (ReferenceEquals(x.DataReaderType, y.DataReaderType) == false)
                {
                    return false;
                }

                return true;
            }

            public int GetHashCode(CacheItem obj)
            {
                unchecked
                {
                    if (obj.ColumnNames.Length == 0)
                    {
                        return obj.DataReaderType.GetHashCode() ^ obj.Type.GetHashCode() ^ obj.ColumnTypes[0].GetHashCode();
                    }

                    return obj.DataReaderType.GetHashCode() ^ obj.Type.GetHashCode() ^ obj.ColumnNames.Length ^ obj.ColumnNames[0].GetHashCode() ^ obj.ColumnNames[obj.ColumnNames.Length - 1].GetHashCode();
                }
            }
        }
    }
}
