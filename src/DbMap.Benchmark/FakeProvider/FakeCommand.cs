using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace DbMap.Benchmark.FakeProvider
{
    internal class FakeCommand : DbCommand
    {
        private static readonly Dictionary<string, Tuple<MethodInfo, MethodInfo, Type, PropertyInfo[]>> ReaderMethodCache = new Dictionary<string, Tuple<MethodInfo, MethodInfo, Type, PropertyInfo[]>>();
        private static readonly Dictionary<string, MethodInfo> ScalarMethodCache = new Dictionary<string, MethodInfo>();

        private DbParameterCollection parameterCollection;

        public override string CommandText { get; set; }

        public override int CommandTimeout { get; set; }

        public override CommandType CommandType { get; set; }

        public override bool DesignTimeVisible { get; set; }

        public override UpdateRowSource UpdatedRowSource { get; set; }

        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection
        {
            get
            {
                return parameterCollection ??= new FakeParameterCollection();
            }
        }

        protected override DbTransaction DbTransaction { get; set; }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            if (ScalarMethodCache.TryGetValue(CommandText, out var cached) == false)
            {
                var lastDotIndex = CommandText.LastIndexOf('.');

                var typeString = CommandText.Substring(0, lastDotIndex);
                var type = Assembly.GetExecutingAssembly().GetType(typeString);

                var methodString = CommandText.Substring(lastDotIndex + 1);
                cached = type.GetMethod(methodString);

                ScalarMethodCache.Add(CommandText, cached);
            }

            var value = cached.Invoke(null, null);
            return value ?? DBNull.Value;
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            return new FakeParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            if (ReaderMethodCache.TryGetValue(CommandText, out var cached) == false)
            {
                var lastDotIndex = CommandText.LastIndexOf('.');

                var typeString = CommandText.Substring(0, lastDotIndex);
                var type = Assembly.GetExecutingAssembly().GetType(typeString);

                var methodString = CommandText.Substring(lastDotIndex + 1);
                var methodInfo = type.GetMethod(methodString);
                var returnType = methodInfo.ReturnType;

                var enumeratorMethodInfo = returnType.GetMethod("GetEnumerator");

                var rowType = returnType.GetGenericArguments()[0];

                if (IsClrType(Nullable.GetUnderlyingType(rowType) ?? rowType))
                {
                    cached = Tuple.Create(methodInfo, enumeratorMethodInfo, rowType, (PropertyInfo[])null);
                }
                else
                {
                    cached = Tuple.Create(methodInfo, enumeratorMethodInfo, (Type)null, rowType.GetProperties());
                }

                ReaderMethodCache.Add(CommandText, cached);
            }

            var enumerable = cached.Item1.Invoke(null, null);
            var enumerator = (IEnumerator)cached.Item2.Invoke(enumerable, null);

            return new FakeDataReader(cached.Item3, cached.Item4, enumerator);
        }

        private static bool IsClrType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            switch (Type.GetTypeCode(underlyingType))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.DateTime:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
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
    }
}
