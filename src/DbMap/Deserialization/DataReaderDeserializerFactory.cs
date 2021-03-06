﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;

using DbMap.Infrastructure;

namespace DbMap.Deserialization
{
    internal static class DataReaderDeserializerFactory
    {
        private static readonly Type DbDataReaderType = typeof(DbDataReader);
        private static readonly Type[] DeserializeAllParameters = { DbCommandMetadata.Type, DbDataReaderType };
        private static readonly Type[] DbDataReaderTypeArray = { DbDataReaderType };

        internal static DataReaderDeserializer Create(Type connectionType, Type type, string[] columnNames, Type[] columnTypes)
        {
            var adoProviderMetadata = AdoProviderMetadata.GetMetadata(connectionType);
            var dataReaderMetadata = adoProviderMetadata.DataReaderMetadata;

            var moduleBuilder = DynamicAssembly.GetExistingDynamicAssemblyOrCreateNew(type.Assembly);

            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(type);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(type);
            var dataReaderDeserializerType = typeof(DataReaderDeserializer);
            var typeName = DynamicAssembly.GetUniqueTypeName("DbMap.Runtime." + type.Name + nameof(DataReaderDeserializer));

            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Sealed, dataReaderDeserializerType, new[] { enumeratorType, enumerableType });

            var fieldMap = new FieldMap(typeBuilder);

            // Fields
            var readerField = typeBuilder.DefineField("reader", DbDataReaderType, FieldAttributes.Private);
            var commandField = typeBuilder.DefineField("command", typeof(DbCommand), FieldAttributes.Private);

            // .ctor()
            {
                var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
                var il = constructor.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Ret);
            }

            // .ctor(DbCommand, DbDataReader)
            var dataReaderDeserializerConstructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, DeserializeAllParameters);
            {
                var il = dataReaderDeserializerConstructor.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, commandField);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Stfld, readerField);
                il.Emit(OpCodes.Ret);
            }

            // Reset()
            {
                var method = typeBuilder.DefineMethod("Reset", MethodAttributes.Public | MethodAttributes.Virtual);
                var il = method.GetILGenerator();
                il.ThrowException(typeof(NotSupportedException));
                il.Emit(OpCodes.Ret);
            }

            // Dispose()
            {
                var method = typeBuilder.DefineMethod("Dispose", MethodAttributes.Public | MethodAttributes.Virtual);
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, readerField);
                il.Emit(OpCodes.Call, dataReaderMetadata.DisposeMethod);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, commandField);
                il.Emit(OpCodes.Call, DbCommandMetadata.Dispose);
                il.Emit(OpCodes.Ret);
            }

            // MoveNext()
            {
                var method = typeBuilder.DefineMethod("MoveNext", MethodAttributes.Public | MethodAttributes.Virtual, typeof(bool), Type.EmptyTypes);
                var il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, readerField);
                il.Emit(OpCodes.Call, dataReaderMetadata.ReadMethod);
                il.Emit(OpCodes.Ret);
            }

            // TReturn Deserialize<TReturn>(DBDataReader)
            var deserializeMethod = typeBuilder.DefineMethod(nameof(DataReaderDeserializer.Deserialize), MethodAttributes.Public | MethodAttributes.Virtual);
            {
                var returnTypeParameter = deserializeMethod.DefineGenericParameters("TReturn")[0];

                deserializeMethod.SetReturnType(returnTypeParameter);
                deserializeMethod.SetParameters(DbDataReaderTypeArray);

                var il = deserializeMethod.GetILGenerator();
                EmitDeserializeType(dataReaderMetadata, fieldMap, il, type, columnNames, columnTypes);
                il.Emit(OpCodes.Ret);
            }

            // IEnumerable<TReturn> DeserializeAll<TReturn>(DbCommand, DbDataReader)
            {
                var deserializeAllMethod = typeBuilder.DefineMethod(nameof(DataReaderDeserializer.DeserializeAll), MethodAttributes.Public | MethodAttributes.Virtual);
                var returnTypeParameter = deserializeAllMethod.DefineGenericParameters("TReturn")[0];

                deserializeAllMethod.SetReturnType(typeof(IEnumerable<>).MakeGenericType(returnTypeParameter));
                deserializeAllMethod.SetParameters(DeserializeAllParameters);

                var il = deserializeAllMethod.GetILGenerator();
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Newobj, dataReaderDeserializerConstructor);
                il.Emit(OpCodes.Ret);
            }

            // TReturn Current { get; }
            {
                var property = typeBuilder.DefineProperty("Current", PropertyAttributes.None, type, Type.EmptyTypes);
                var propertyGet = typeBuilder.DefineMethod("get_Current", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual, type, Type.EmptyTypes);
                var il = propertyGet.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, readerField);
                il.Emit(OpCodes.Call, deserializeMethod.MakeGenericMethod(type));
                il.Emit(OpCodes.Ret);

                property.SetGetMethod(propertyGet);
            }

            // object Current { get; }
            {
                var property = typeBuilder.DefineProperty("Current", PropertyAttributes.HasDefault, typeof(object), Type.EmptyTypes);
                var propertyGet = typeBuilder.DefineMethod("get_Current", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual, typeof(object), Type.EmptyTypes);
                var il = propertyGet.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, readerField);
                il.Emit(OpCodes.Call, deserializeMethod.MakeGenericMethod(type));
                il.Emit(OpCodes.Castclass, typeof(object));
                il.Emit(OpCodes.Ret);

                property.SetGetMethod(propertyGet);
            }

            // IEnumerator<TReturn> GetEnumerator()
            {
                var method = typeBuilder.DefineMethod("GetEnumerator", MethodAttributes.Public | MethodAttributes.Virtual, enumeratorType, Type.EmptyTypes);
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ret);
            }

            // IEnumerator GetEnumerator()
            {
                var method = typeBuilder.DefineMethod("GetEnumerator", MethodAttributes.Public | MethodAttributes.Virtual, typeof(IEnumerator), Type.EmptyTypes);
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, typeof(IEnumerator));
                il.Emit(OpCodes.Ret);
            }

            return (DataReaderDeserializer)Activator.CreateInstance(typeBuilder.CreateTypeInfo());
        }

        private static void EmitDeserializeType(DataReaderMetadata dataReaderMetadata, FieldMap fieldMap, ILGenerator il, Type type, string[] columnNames, Type[] columnTypes)
        {
            if (columnNames == DbQuery.NoColumnNames)
            {
                EmitDeserializeClrType(dataReaderMetadata, fieldMap, il, columnTypes[0], type);
            }
            else
            {
                EmitDeserializeUserType(dataReaderMetadata, fieldMap, il, type, columnNames, columnTypes);
            }
        }

        private static void EmitDeserializeClrType(DataReaderMetadata dataReaderMetadata, FieldMap fieldMap, ILGenerator il, Type sourceType, Type type)
        {
            var getValueMethod = dataReaderMetadata.GetGetValueMethodFromType(sourceType);

            new DataReaderValueDeserializer(dataReaderMetadata, getValueMethod, type, 0).EmitDeserializeClrType(il, fieldMap, false);
        }

        private static bool PropertyHasRequiredAttribute(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsClass == false)
            {
                return false;
            }

            foreach (var customAttributes in propertyInfo.GetCustomAttributes())
            {
                var type = customAttributes.GetType();
                if (type.Namespace == "System.ComponentModel.DataAnnotations" && type.Name == "RequiredAttribute")
                {
                    return true;
                }
            }

            return false;
        }

        private static void EmitDeserializeUserType(DataReaderMetadata dataReaderMetadata, FieldMap fieldMap, ILGenerator il, Type type, string[] columnNames, Type[] columnTypes)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                ThrowException.NoDefaultConstructor();
            }

            var sample = Activator.CreateInstance(type);
            il.Emit(OpCodes.Newobj, constructor);

            var uniqueNames = new HashSet<string>();
            for (var ordinal = 0; ordinal < columnNames.Length; ordinal++)
            {
                var propertyInfo = type.GetProperty(columnNames[ordinal], BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (propertyInfo == null)
                {
                    continue;
                }

                var getValueMethod = dataReaderMetadata.GetGetValueMethodFromType(columnTypes[ordinal]);

                if (propertyInfo.CanWrite && propertyInfo.GetSetMethod() != null)
                {
                    var propertyIsNullInitialized = propertyInfo.CanRead && propertyInfo.GetValue(sample) == null;
                    var hasRequiredAttribute = PropertyHasRequiredAttribute(propertyInfo);
                    var dataReaderPropertyValueDeserializer = new DataReaderValueDeserializer(dataReaderMetadata, getValueMethod, propertyInfo, ordinal);
                    dataReaderPropertyValueDeserializer.EmitDeserializeProperty(il, fieldMap, propertyIsNullInitialized, hasRequiredAttribute);

                    if (uniqueNames.Add(propertyInfo.Name) == false)
                    {
                        ThrowException.DuplicateFieldNames(propertyInfo.Name);
                    }

                    continue;
                }

                var fieldInfo = type.GetField("<" + propertyInfo.Name + ">k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fieldInfo != null)
                {
                    var fieldIsNullInitialized = fieldInfo.GetValue(sample) == null;
                    var hasRequiredAttribute = PropertyHasRequiredAttribute(propertyInfo);
                    var dataReaderFieldValueDeserializer = new DataReaderValueDeserializer(dataReaderMetadata, getValueMethod, fieldInfo, ordinal);
                    dataReaderFieldValueDeserializer.EmitDeserializeProperty(il, fieldMap, fieldIsNullInitialized, hasRequiredAttribute);

                    if (uniqueNames.Add(propertyInfo.Name) == false)
                    {
                        ThrowException.DuplicateFieldNames(propertyInfo.Name);
                    }
                }
            }
        }
    }
}
