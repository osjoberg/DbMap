using System;
using System.Data.Common;
using System.Reflection;

namespace DbMap.Deserialization
{
    internal class DataReaderMetadata
    {
        private const BindingFlags PublicInstanceDeclared = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private static readonly Type[] OrdinalParameters = { typeof(int) };

        private readonly MethodInfo getBooleanMethod;
        private readonly MethodInfo getByteMethod;
        private readonly MethodInfo getCharMethod;
        private readonly MethodInfo getDateTimeMethod;
        private readonly MethodInfo getDecimalMethod;
        private readonly MethodInfo getDoubleMethod;
        private readonly MethodInfo getFloatMethod;
        private readonly MethodInfo getGuidMethod;
        private readonly MethodInfo getInt16Method;
        private readonly MethodInfo getInt32Method;
        private readonly MethodInfo getInt64Method;
        private readonly MethodInfo getStringMethod;
        private readonly MethodInfo getValueMethod;

        private readonly MethodInfo getSByteMethod;
        private readonly MethodInfo getUInt16Method;
        private readonly MethodInfo getUInt32Method;
        private readonly MethodInfo getUInt64Method;

        public DataReaderMetadata(Type dataReaderType)
        {
            DisposeMethod = dataReaderType.GetMethod(nameof(DbDataReader.Dispose), Type.EmptyTypes);
            ReadMethod = dataReaderType.GetMethod(nameof(DbDataReader.Read), PublicInstanceDeclared);
            IsDBNullMethod = dataReaderType.GetMethod(nameof(DbDataReader.IsDBNull), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);

            getBooleanMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetBoolean), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getByteMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetByte), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getCharMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetChar), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getDateTimeMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetDateTime), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getDecimalMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetDecimal), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getDoubleMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetDouble), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getFloatMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetFloat), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getGuidMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetGuid), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getInt16Method = dataReaderType.GetMethod(nameof(DbDataReader.GetInt16), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getInt32Method = dataReaderType.GetMethod(nameof(DbDataReader.GetInt32), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getInt64Method = dataReaderType.GetMethod(nameof(DbDataReader.GetInt64), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getStringMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetString), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getValueMethod = dataReaderType.GetMethod(nameof(DbDataReader.GetValue), PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);

            getSByteMethod = dataReaderType.GetMethod("GetSByte", PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getUInt16Method = dataReaderType.GetMethod("GetUInt16", PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getUInt32Method = dataReaderType.GetMethod("GetUInt32", PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
            getUInt64Method = dataReaderType.GetMethod("GetUInt64", PublicInstanceDeclared, null, CallingConventions.Any, OrdinalParameters, null);
        }

        public MethodInfo ReadMethod { get; }

        public MethodInfo DisposeMethod { get; }

        public MethodInfo IsDBNullMethod { get; }

        public MethodInfo GetGetValueMethodFromType(Type sourceType)
        {
            switch (Type.GetTypeCode(sourceType))
            {
                case TypeCode.Boolean:
                    return getBooleanMethod;
                case TypeCode.Char:
                    return getCharMethod;
                case TypeCode.SByte:
                    return getSByteMethod;
                case TypeCode.Byte:
                    return getByteMethod;
                case TypeCode.Int16:
                    return getInt16Method;
                case TypeCode.UInt16:
                    return getUInt16Method;
                case TypeCode.Int32:
                    return getInt32Method;
                case TypeCode.UInt32:
                    return getUInt32Method;
                case TypeCode.Int64:
                    return getInt64Method;
                case TypeCode.UInt64:
                    return getUInt64Method;
                case TypeCode.Single:
                    return getFloatMethod;
                case TypeCode.Double:
                    return getDoubleMethod;
                case TypeCode.Decimal:
                    return getDecimalMethod;
                case TypeCode.DateTime:
                    return getDateTimeMethod;
                case TypeCode.String:
                    return getStringMethod;
                default:

                    if (ReferenceEquals(sourceType, typeof(Guid)))
                    {
                        return getGuidMethod;
                    }

                    if (sourceType.IsArray)
                    {
                        return getValueMethod;
                    }

                    return null;
            }
        }
    }
}