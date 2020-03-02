using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DbMap.Infrastructure
{
    internal static class DynamicAssembly
    {
        private static readonly ConstructorInfo IgnoresAccessChecksToAttributeConstructor = typeof(IgnoresAccessChecksToAttribute).GetConstructor(new[] { typeof(string) });
        private static readonly object DynamicAssemblyLock = new object();

        private static int typeId;

        private static Tuple<Assembly, ModuleBuilder>[] moduleBuilders = { };

        public static ModuleBuilder GetExistingDynamicAssemblyOrCreateNew(Assembly assembly)
        {
            var moduleBuildersCopy = moduleBuilders;

            for (var index = 0; index < moduleBuildersCopy.Length; index++)
            {
                var (moduleBuilderAssembly, moduleBuilder) = moduleBuildersCopy[index];
                if (ReferenceEquals(moduleBuilderAssembly, assembly))
                {
                    return moduleBuilder;
                }
            }
            
            lock (DynamicAssemblyLock)
            {
                moduleBuildersCopy = moduleBuilders;
                for (var index = 0; index < moduleBuildersCopy.Length; index++)
                {
                    var (moduleBuilderAssembly, moduleBuilder) = moduleBuildersCopy[index];
                    if (ReferenceEquals(moduleBuilderAssembly, assembly))
                    {
                        return moduleBuilder;
                    }
                }

                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Daf.Map.Runtime"), AssemblyBuilderAccess.Run, Array.Empty<CustomAttributeBuilder>());
                var assemblyName = assembly.GetName().Name;
                var builder = new CustomAttributeBuilder(IgnoresAccessChecksToAttributeConstructor, new object[] { assemblyName });
                assemblyBuilder.SetCustomAttribute(builder);

                var newModuleBuilder = assemblyBuilder.DefineDynamicModule("Daf.Map.Runtime");
                var newModuleBuilders = new Tuple<Assembly, ModuleBuilder>[moduleBuildersCopy.Length + 1];
                Array.Copy(moduleBuildersCopy, 0, newModuleBuilders, 1, moduleBuildersCopy.Length);
                newModuleBuilders[0] = Tuple.Create(assembly, newModuleBuilder);
                moduleBuilders = newModuleBuilders;

                return newModuleBuilder;
            }
        }

        public static string GetUniqueTypeName(string typeName)
        {
            return typeName + Interlocked.Increment(ref typeId);
        }
    }
}
