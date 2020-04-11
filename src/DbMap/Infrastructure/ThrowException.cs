using System;
using System.Reflection;

namespace DbMap.Infrastructure
{
    internal static class ThrowException
    {
        internal static readonly MethodInfo NotSupportedMethod = typeof(ThrowException).GetMethod(nameof(NotSupported));

        public static void SequenceContainsNoElements()
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        public static void SequenceContainsMoreThanOneElement()
        {
            throw new InvalidOperationException("Sequence contains more than one element");
        }

        public static void ValueCannotBeNull(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        public static void NoDefaultConstructor()
        {
            throw new InvalidOperationException("A default, parameterless constructor is required to be able to materialize type.");
        }

        public static void DuplicateFieldNames(string fieldName)
        {
            throw new InvalidOperationException($"Field name '{fieldName}' is not unique which is required to be able to materialize type.");
        }

        public static void NotSupported()
        {
            throw new NotSupportedException();
        }

        public static string InvalidCast(Type from, Type to)
        {
            return $"Invalid cast from '{from.Name}' to '{to.Name}'.";
        }
    }
}
