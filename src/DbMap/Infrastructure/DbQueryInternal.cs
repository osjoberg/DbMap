using System;
using System.Data.Common;

using DbMap.Deserialization;

namespace DbMap.Infrastructure
{
    internal static class DbQueryInternal
    {
        public static bool IsClrType(Type type)
        {
            var underlyingType = NullableInfo.GetNullable(type)?.UnderlyingType ?? type;
            switch (Type.GetTypeCode(underlyingType))
            {
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.DateTime:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.String:
                    return true;

                default:
                    if (underlyingType.IsArray)
                    {
                        return true;
                    }

                    if (ReferenceEquals(underlyingType, typeof(Guid)))
                    {
                        return true;
                    }

                    return false;
            }
        }

        public static string[] GetColumnNames(DbDataReader reader)
        {
            var columnNames = new string[reader.FieldCount];
            for (var ordinal = 0; ordinal < columnNames.Length; ordinal++)
            {
                columnNames[ordinal] = reader.GetName(ordinal);
            }

            return columnNames;
        }

        public static Type[] GetColumnTypes(DbDataReader reader)
        {
            var columnTypes = new Type[reader.FieldCount];
            for (var ordinal = 0; ordinal < columnTypes.Length; ordinal++)
            {
                columnTypes[ordinal] = reader.GetFieldType(ordinal);
            }

            return columnTypes;
        }

        public static TReturn QueryFirst<TReturn>(DbDataReader reader, DataReaderDeserializer dataReaderDeserializer)
        {
            if (reader.Read() == false)
            {
                ThrowException.SequenceContainsNoElements();
            }

            return dataReaderDeserializer.Deserialize<TReturn>(reader);
        }

        public static TReturn QueryFirstOrDefault<TReturn>(DbDataReader reader, DataReaderDeserializer dataReaderDeserializer)
        {
            if (reader.Read() == false)
            {
                return default;
            }

            return dataReaderDeserializer.Deserialize<TReturn>(reader);
        }

        public static TReturn QuerySingle<TReturn>(DbDataReader reader, DataReaderDeserializer dataReaderDeserializer)
        {
            if (reader.Read() == false)
            {
                ThrowException.SequenceContainsNoElements();
            }

            var entity = dataReaderDeserializer.Deserialize<TReturn>(reader);

            if (reader.Read())
            {
                ThrowException.SequenceContainsMoreThanOneElement();
            }

            return entity;
        }

        public static TReturn QuerySingleOrDefault<TReturn>(DbDataReader reader, DataReaderDeserializer dataReaderDeserializer)
        {
            if (reader.Read() == false)
            {
                return default;
            }

            var entity = dataReaderDeserializer.Deserialize<TReturn>(reader);

            if (reader.Read())
            {
                ThrowException.SequenceContainsMoreThanOneElement();
            }

            return entity;
        }
    }
}
