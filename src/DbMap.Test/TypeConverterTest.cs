using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using DbMap.Deserialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test
{
    [TestClass]
    public class TypeConverterTest
    {
        [TestMethod]
        [DynamicData(nameof(GetNumericTypes), DynamicDataSourceType.Method)]
        public void CanConvertZeroToBooleanFalse(Type from)
        {
            InternalCanConvertValue(from, typeof(bool), ChangeType(0, from), false);
        }

        [TestMethod]
        [DynamicData(nameof(GetNumericTypes), DynamicDataSourceType.Method)]
        public void CanConvertNonZeroToBooleanTrue(Type from)
        {
            InternalCanConvertValue(from, typeof(bool), ChangeType(127, from), true);
        }

        [TestMethod]
        [DynamicData(nameof(GetNumericTypes), DynamicDataSourceType.Method)]
        public void CanConvertBooleanFalseToZero(Type to)
        {
            InternalCanConvertValue(typeof(bool), to, false, ChangeType(0, to));
        }

        [TestMethod]
        [DynamicData(nameof(GetNumericTypes), DynamicDataSourceType.Method)]
        public void CanConvertBooleanTrueToOne(Type to)
        {
            InternalCanConvertValue(typeof(bool), to, true, ChangeType(1, to));
        }

        [TestMethod]
        [DynamicData(nameof(GetNumericTypes), DynamicDataSourceType.Method)]
        public void CanConvertToObject(Type from)
        {
            InternalCanConvertValue(from, typeof(object), ChangeType(127, from), ChangeType(127, from));
        }

        [TestMethod]
        [DynamicData(nameof(GetNumericTypeConversions), DynamicDataSourceType.Method)]
        public void CanConvertDefaultValue(Type from, Type to)
        {
            InternalCanConvertValue(from, to, ChangeType(0, from), ChangeType(0, to));
        }

        [TestMethod]
        [DynamicData(nameof(GetNumericTypeConversions), DynamicDataSourceType.Method)]
        public void CanConvertValue(Type from, Type to)
        {
            InternalCanConvertValue(from, to, ChangeType(127, from), from == typeof(bool) ? ChangeType(1, to) : ChangeType(127, to));
        }

        [TestMethod]
        [DynamicData(nameof(GetNumericTypeConversions), DynamicDataSourceType.Method)]
        public void CanConvertMaxValueOrThrowOverflowException(Type from, Type to)
        {
            var fromMaxValue = from.GetField("MaxValue").GetValue(null);
            var toMaxValue = to.GetField("MaxValue").GetValue(null);

            var fromMaxValueDouble = (double)ChangeType(fromMaxValue, typeof(double));
            var toMaxValueDouble = (double)ChangeType(toMaxValue, typeof(double));

            var isDoubleToSingleConversation = from == typeof(double) && to == typeof(float);
            if (isDoubleToSingleConversation)
            {
                InternalCanConvertValue(from, to, fromMaxValue, float.PositiveInfinity);
                return;
            }

            var expectOverflow = fromMaxValueDouble > toMaxValueDouble;
            if (expectOverflow)
            {
                InternalConvertValueThrowsOverflowException(from, to, fromMaxValue);
                return;
            }

            InternalCanConvertValue(from, to, fromMaxValue, ChangeType(fromMaxValue, to));
        }

        [TestMethod]
        [DynamicData(nameof(GetNumericTypeConversions), DynamicDataSourceType.Method)]
        public void CanConvertMinValueOrThrowOverflowException(Type from, Type to)
        {
            var fromMinValue = from.GetField("MinValue").GetValue(null);
            var toMinValue = to.GetField("MinValue").GetValue(null);

            var fromMinValueDouble = (double)ChangeType(fromMinValue, typeof(double));
            var toMinValueDouble = (double)ChangeType(toMinValue, typeof(double));

            var isDoubleToSingleConversation = from == typeof(double) && to == typeof(float);
            if (isDoubleToSingleConversation)
            {
                InternalCanConvertValue(from, to, fromMinValue, float.NegativeInfinity);
                return;
            }

            var expectOverflow = fromMinValueDouble < toMinValueDouble;
            if (expectOverflow)
            {
                InternalConvertValueThrowsOverflowException(from, to, fromMinValue);
                return;
            }

            InternalCanConvertValue(from, to, fromMinValue, ChangeType(fromMinValue, to));
        }

        private static void InternalCanConvertValue(Type from, Type to, object fromValue, object toValue)
        {
            var conversionMethod = BuildConversionMethod(from, to);

            var actual = conversionMethod.Invoke(null, new[] { fromValue });

            Assert.AreEqual(toValue, actual); 
        }

        private static void InternalConvertValueThrowsOverflowException(Type from, Type to, object fromValue)
        {
            var conversionMethod = BuildConversionMethod(from, to);

            try
            {
                conversionMethod.Invoke(null, new[] { fromValue });
                Assert.Fail("Expected TargetInvocationException with child OverflowException");
            }
            catch (Exception exception)
            {
                if (exception is TargetInvocationException == false || exception.InnerException is OverflowException == false)
                {
                    throw;
                }
            }
        }

        private static DynamicMethod BuildConversionMethod(Type from, Type to)
        {
            var conversionMethod = new DynamicMethod($"ConvertFrom{from.Name}To{to.Name}", to, new[] { from });
            var il = conversionMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);

            TypeConverter.EmitConversion(il, from, to);

            il.Emit(OpCodes.Ret);

            return conversionMethod;
        }

        private static object ChangeType(object value, Type type)
        {
            if (type == typeof(char) || value is char)
            {
                value = Convert.ChangeType(value, typeof(int));
            }

            return Convert.ChangeType(value, type);
        }

        private static IEnumerable<object[]> GetNumericTypes()
        {
            var types = new[]
            {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(char),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal)
            };

            return types.Select(type => new object[] { type });
        }

        private static IEnumerable<object[]> GetNumericTypeConversions()
        {
            return GetNumericTypes().SelectMany(from => GetNumericTypes().Select(to => new[] { from[0], to[0] }));
        }
    }
}
