using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace DbMap.Infrastructure
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal class NullableInfo
    {
        private static readonly TypeComparer TypeComparerInstance = new TypeComparer();

        private static Dictionary<Type, NullableInfo> nullableInfos = new Dictionary<Type, NullableInfo>(TypeComparerInstance);

        private NullableInfo(Type type, Type underlyingType)
        {
            UnderlyingType = underlyingType;
            Constructor = type.GetConstructor(new[] { underlyingType });
            HasValueGetProperty = type.GetProperty(nameof(Nullable<int>.HasValue)).GetGetMethod();
            GetValueOrDefaultMethod = type.GetMethod(nameof(Nullable<int>.GetValueOrDefault), Type.EmptyTypes);
        }

        public MethodInfo GetValueOrDefaultMethod { get; set; }

        public MethodInfo HasValueGetProperty { get; set; }

        public Type UnderlyingType { get; }

        public ConstructorInfo Constructor { get; }

        public static NullableInfo GetNullable(Type type)
        {
            if (nullableInfos.TryGetValue(type, out var value))
            {
                return value;
            }

            var underlyingType = Nullable.GetUnderlyingType(type);
            var nullableInfo = underlyingType == null ? null : new NullableInfo(type, underlyingType);

            nullableInfos = new Dictionary<Type, NullableInfo>(nullableInfos, TypeComparerInstance) { { type, nullableInfo } };

            return nullableInfo;
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
