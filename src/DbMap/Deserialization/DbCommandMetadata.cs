using System;
using System.Data.Common;
using System.Reflection;

namespace DbMap.Deserialization
{
    internal class DbCommandMetadata
    {
        public static readonly Type Type = typeof(DbCommand);
        public static readonly MethodInfo Dispose = Type.GetMethod(nameof(DbCommand.Dispose));
    }
}
