using System;
using System.Collections;
using System.Data.Common;
using System.Reflection;

namespace DbMap.Benchmark.FakeProvider
{
    internal class FakeDataReader : DbDataReader
    {
        private readonly Type type;
        private readonly IEnumerator rowEnumerator;
        private readonly PropertyInfo[] propertyInfos;

        private bool hasRows;

        public FakeDataReader(Type type, PropertyInfo[] propertyInfos, IEnumerator rowEnumerator)
        {
            this.type = Nullable.GetUnderlyingType(type) ?? type;
            this.propertyInfos = propertyInfos;
            this.rowEnumerator = rowEnumerator;
            hasRows = true;
        }

        public override int Depth { get; } = 0;

        public override int FieldCount => propertyInfos.Length;

        public override bool HasRows => hasRows;

        public override bool IsClosed { get; } = false;

        public override int RecordsAffected { get; } = 0;

        public override object this[string name] => throw new NotImplementedException();

        public override object this[int ordinal] => throw new NotImplementedException();

        public override bool GetBoolean(int ordinal)
        {
            return GetValue<bool>(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return GetValue<byte>(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            return GetValue<char>(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return GetValue<DateTime>(ordinal);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return GetValue<decimal>(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return GetValue<double>(ordinal);
        }

        public override Type GetFieldType(int ordinal)
        {
            return propertyInfos[ordinal].PropertyType;
        }

        public override float GetFloat(int ordinal)
        {
            return GetValue<float>(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return GetValue<Guid>(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return GetValue<short>(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return GetValue<int>(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return GetValue<long>(ordinal);
        }

        public override string GetName(int ordinal)
        {
            return propertyInfos[ordinal].Name;
        }

        public override int GetOrdinal(string name)
        {
            for (var ordinal = 0; ordinal < propertyInfos.Length; ordinal++)
            {
                if (propertyInfos[ordinal].Name == name)
                {
                    return ordinal;
                }
            }
            
            throw new Exception();
        }

        public override string GetString(int ordinal)
        {
            return GetValue<string>(ordinal);
        }

        public override object GetValue(int ordinal)
        {
            return GetValue<object>(ordinal);
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            if (propertyInfos == null)
            {
                return rowEnumerator.Current == null;
            }

            return propertyInfos[ordinal].GetValue(rowEnumerator.Current) == null;
        }

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override bool Read()
        {
            hasRows = rowEnumerator.MoveNext();
            return hasRows;
        }

        public override IEnumerator GetEnumerator()
        {
            return rowEnumerator;
        }

        private TValue GetValue<TValue>(int ordinal)
        {
            if (propertyInfos == null)
            {
                if (ReferenceEquals(type, typeof(TValue)) == false)
                {
                    throw new Exception();
                }

                var clrValue = rowEnumerator.Current;
                if (clrValue == null)
                {
                    throw new Exception();
                }

                return (TValue)clrValue;
            }

            var propertyInfo = propertyInfos[ordinal];

            if (ReferenceEquals(propertyInfo.PropertyType, typeof(TValue)) == false && ReferenceEquals(Nullable.GetUnderlyingType(propertyInfo.PropertyType), typeof(TValue)) == false)
            {
                throw new Exception();
            }

            var value = propertyInfo.GetValue(rowEnumerator.Current);
            if (value == null)
            {
                throw new Exception();
            }

            return (TValue)value;
        }
    }
}
