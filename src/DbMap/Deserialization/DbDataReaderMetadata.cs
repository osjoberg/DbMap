using System;
using System.Data.Common;
using System.Reflection;

namespace DbMap.Deserialization
{
    internal static class DbDataReaderMetadata
    {
        public static readonly Type Type = typeof(DbDataReader);
        public static readonly Type[] TypeArray = { Type };
        public static readonly Type[] OrdinalParameters = { typeof(int) };

        public static readonly MethodInfo IsDBNull = Type.GetMethod(nameof(DbDataReader.IsDBNull));
        public static readonly MethodInfo Read = Type.GetMethod(nameof(DbDataReader.Read));
        public static readonly MethodInfo Dispose = Type.GetMethod(nameof(DbDataReader.Dispose));

        private static readonly MethodInfo GetBoolean = Type.GetMethod(nameof(DbDataReader.GetBoolean));
        private static readonly MethodInfo GetByte = Type.GetMethod(nameof(DbDataReader.GetByte));
        private static readonly MethodInfo GetChar = Type.GetMethod(nameof(DbDataReader.GetChar));
        private static readonly MethodInfo GetDateTime = Type.GetMethod(nameof(DbDataReader.GetDateTime));
        private static readonly MethodInfo GetDecimal = Type.GetMethod(nameof(DbDataReader.GetDecimal));
        private static readonly MethodInfo GetDouble = Type.GetMethod(nameof(DbDataReader.GetDouble));
        private static readonly MethodInfo GetInt16 = Type.GetMethod(nameof(DbDataReader.GetInt16));
        private static readonly MethodInfo GetInt32 = Type.GetMethod(nameof(DbDataReader.GetInt32));
        private static readonly MethodInfo GetInt64 = Type.GetMethod(nameof(DbDataReader.GetInt64));
        private static readonly MethodInfo GetFloat = Type.GetMethod(nameof(DbDataReader.GetFloat));
        private static readonly MethodInfo GetString = Type.GetMethod(nameof(DbDataReader.GetString));
        private static readonly MethodInfo GetGuid = Type.GetMethod(nameof(DbDataReader.GetGuid));
        private static readonly MethodInfo GetValue = Type.GetMethod(nameof(DbDataReader.GetValue));

        private static MethodInfo[] nonStandardGetMethods = new MethodInfo[] { };

        public static MethodInfo GetGetValueMethodFromType(Type dataReaderType, Type sourceType)
        {
            switch (Type.GetTypeCode(sourceType))
            {
                case TypeCode.Boolean:
                    return GetBoolean;

                case TypeCode.Byte:
                    return GetByte;

                case TypeCode.Char:
                    return GetChar;

                case TypeCode.DateTime:
                    return GetDateTime;

                case TypeCode.Decimal:
                    return GetDecimal;

                case TypeCode.Double:
                    return GetDouble;

                case TypeCode.Int16:
                    return GetInt16;

                case TypeCode.Int32:
                    return GetInt32;

                case TypeCode.Int64:
                    return GetInt64;

                case TypeCode.Single:
                    return GetFloat;

                case TypeCode.String:
                    return GetString;

                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return ResolveNonStandardGetMethod(dataReaderType, sourceType);

                default:
                    if (ReferenceEquals(sourceType, typeof(byte[])))
                    {
                        return GetValue;
                    }

                    if (ReferenceEquals(sourceType, typeof(char[])))
                    {
                        return GetValue;
                    }

                    if (ReferenceEquals(sourceType, typeof(Guid)))
                    {
                        return GetGuid;
                    }

                    return null;
            }
        }

        private static MethodInfo ResolveNonStandardGetMethod(Type dataReaderType, Type sourceType)
        {
            var nonStandardGetMethodsCopy = nonStandardGetMethods;

            for (var i = 0; i < nonStandardGetMethodsCopy.Length; i++)
            {
                var methodInfo = nonStandardGetMethodsCopy[i];
                if (ReferenceEquals(methodInfo.ReturnType, sourceType) && ReferenceEquals(methodInfo.DeclaringType, dataReaderType))
                {
                    return methodInfo;
                }
            }

            var newMethodInfo = dataReaderType.GetMethod("Get" + sourceType.Name, OrdinalParameters);
            if (newMethodInfo == null)
            {
                return null;
            }
            
            var newNonStandardGetMethods = new MethodInfo[nonStandardGetMethodsCopy.Length + 1];
            Array.Copy(nonStandardGetMethodsCopy, 0, newNonStandardGetMethods, 1, nonStandardGetMethodsCopy.Length);
            newNonStandardGetMethods[0] = newMethodInfo;

            nonStandardGetMethods = newNonStandardGetMethods;

            return newNonStandardGetMethods[0];
        }
    }
}