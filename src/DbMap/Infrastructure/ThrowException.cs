using System;

namespace DbMap.Infrastructure
{
    internal static class ThrowException
    {
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

        public static void NotSupported()
        {
            throw new NotSupportedException();
        }

        public static void InvalidCast(Type from, Type to)
        {
            throw new InvalidCastException($"Invalid cast from '{from.Name}' to '{to.Name}'.");
        }
    }
}
