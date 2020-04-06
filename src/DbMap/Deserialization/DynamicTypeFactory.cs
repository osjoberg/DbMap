using System;
using System.Reflection;
using System.Reflection.Emit;

using DbMap.Infrastructure;

namespace DbMap.Deserialization
{
    internal static class DynamicTypeFactory
    {
        public static Type Create(string[] columnNames, Type[] columnTypes)
        {
            var moduleBuilder = DynamicAssembly.GetExistingDynamicAssemblyOrCreateNew(Assembly.GetExecutingAssembly());
            var typeName = DynamicAssembly.GetUniqueTypeName("DbMap.Runtime.DynamicType");
            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, null);

            // .ctor()
            {
                var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
                var il = constructor.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Ret);
            }

            // Private fields/get properties.
            for (var index = 0; index < columnNames.Length; index++)
            {
                var columnName = columnNames[index];
                var columnType = columnTypes[index];

                for (var innerIndex = 0; innerIndex < index; innerIndex++)
                {
                    if (columnName == columnNames[innerIndex])
                    {
                        ThrowException.DuplicateFieldNames(columnNames[innerIndex]);
                    }
                }

                if (columnType.IsClass == false)
                {
                    columnType = typeof(Nullable<>).MakeGenericType(columnType);
                }

                var fieldName = "<" + columnName + ">k__BackingField";
                
                var field = typeBuilder.DefineField(fieldName, columnType, FieldAttributes.Private);

                var getMethod = typeBuilder.DefineMethod("get_" + columnName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, columnType, Type.EmptyTypes);
                var il = getMethod.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);
                il.Emit(OpCodes.Ret);

                var property = typeBuilder.DefineProperty(columnName, PropertyAttributes.HasDefault, columnType, null);
                property.SetGetMethod(getMethod);
            }

            return typeBuilder.CreateTypeInfo();
        }
    }
}
