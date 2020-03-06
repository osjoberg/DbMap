using System.Reflection;
using System.Reflection.Emit;

namespace DbMap.Infrastructure
{
    internal static class ILGeneratorExtensions
    {
        public static void Invoke(this ILGenerator il, MethodInfo method, int argument0)
        {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4, argument0);
            il.Emit(OpCodes.Callvirt, method);
        }
    }
}
