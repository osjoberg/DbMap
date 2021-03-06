﻿using System;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;

using DbMap.Deserialization;
using DbMap.Infrastructure;

namespace DbMap.Serialization
{
    internal static class ParametersSerializerFactory
    {
        private static readonly FieldInfo DbNull = typeof(DBNull).GetField(nameof(DBNull.Value));

        internal static ParametersSerializer Create(Type connectionType, Type parametersType)
        {
            var moduleBuilder = DynamicAssembly.GetExistingDynamicAssemblyOrCreateNew(parametersType.Assembly);

            var typeName = DynamicAssembly.GetUniqueTypeName("Daf.Map.Runtime." + parametersType.Name + nameof(ParametersSerializer));
            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Sealed, typeof(ParametersSerializer), Type.EmptyTypes);

            // Serialize().
            {
                var method = typeBuilder.DefineMethod(nameof(ParametersSerializer.Serialize), MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard, typeof(void), new[] { typeof(DbParameterCollection), typeof(object) });
                var il = method.GetILGenerator();

                var locals = new LocalsMap(il);

                var properties = parametersType.GetProperties();
                for (var parameterIndex = 0; parameterIndex < properties.Length; parameterIndex++)
                {
                    var propertyInfo = properties[parameterIndex];

                    if (propertyInfo.CanRead == false || propertyInfo.GetGetMethod() == null)
                    {
                        var backingField = parametersType.GetField("<" + propertyInfo.Name + ">k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (backingField == null)
                        {
                            continue;
                        }

                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldstr, '@' + propertyInfo.Name);
                        il.Emit(OpCodes.Ldarg_2);
                        il.Emit(OpCodes.Ldfld, backingField);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldstr, '@' + propertyInfo.Name);
                        il.Emit(OpCodes.Ldarg_2);
                        il.Emit(OpCodes.Call, propertyInfo.GetGetMethod());
                    }

                    var nullableInfo = NullableInfo.GetNullable(propertyInfo.PropertyType);
                    if (nullableInfo != null)
                    {
                        var isNotNullLabel = il.DefineLabel();
                        var completeLabel = il.DefineLabel();

                        var index = locals.GetLocalIndex(propertyInfo.PropertyType, "nullable") ?? locals.DeclareLocal(propertyInfo.PropertyType, "nullable");
                        il.Emit(OpCodes.Stloc, index);
                        il.Emit(OpCodes.Ldloca_S, index);

                        il.Emit(OpCodes.Call, nullableInfo.HasValueGetProperty);
                        il.Emit(OpCodes.Brtrue_S, isNotNullLabel);

                        // Value == null
                        {
                            il.Emit(OpCodes.Ldsfld, DbNull);
                            il.Emit(OpCodes.Br_S, completeLabel);
                        }

                        // Value != null
                        {
                            il.MarkLabel(isNotNullLabel);
                            il.Emit(OpCodes.Ldloca_S, index);
                            il.Emit(OpCodes.Call, nullableInfo.GetValueOrDefaultMethod);
                            il.Emit(OpCodes.Box, nullableInfo.UnderlyingType);
                        }

                        il.MarkLabel(completeLabel);
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        var completeLabel = il.DefineLabel();

                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Brtrue_S, completeLabel);

                        // Value == null
                        {
                            il.Emit(OpCodes.Pop);
                            il.Emit(OpCodes.Ldsfld, DbNull);
                        }

                        il.MarkLabel(completeLabel);
                    }
                    else
                    {
                        il.Emit(OpCodes.Box, propertyInfo.PropertyType);
                    }

                    var adoProviderMetadata = AdoProviderMetadata.GetMetadata(connectionType);

                    il.Emit(OpCodes.Newobj, adoProviderMetadata.ParameterConstructor);
                    il.Emit(OpCodes.Call, adoProviderMetadata.ParameterCollectionAddMethod);
                    il.Emit(OpCodes.Pop);
                }

                il.Emit(OpCodes.Ret);
            }

            return (ParametersSerializer)Activator.CreateInstance(typeBuilder.CreateTypeInfo());
        }
    }
}
