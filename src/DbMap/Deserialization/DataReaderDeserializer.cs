using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;

using DbMap.Infrastructure;

namespace DbMap.Deserialization
{
    public abstract class DataReaderDeserializer
    {
        public abstract TReturn Deserialize<TReturn>(DbDataReader reader);

        public abstract IEnumerable<TReturn> DeserializeAll<TReturn>(DbDataReader reader);

        internal static DataReaderDeserializer Create(Type type, string[] columnNames)
        {
            var moduleBuilder = DynamicAssembly.GetExistingDynamicAssemblyOrCreateNew(type.Assembly);

            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(type);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(type);
            var dataReaderDeserializerType = typeof(DataReaderDeserializer);
            var typeName = DynamicAssembly.GetUniqueTypeName("DbMap.Runtime." + type.Name + "DataReaderDeserializer");

            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, dataReaderDeserializerType, new[] { enumeratorType, enumerableType });

            // Fields
            var readerField = typeBuilder.DefineField("reader", DbDataReaderMetadata.Type, FieldAttributes.Private);

            // ctor()
            {
                var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
                var il = constructor.GetILGenerator();
                il.Emit(OpCodes.Ret);
            }

            // .ctor(IDataReader)
            var dataReaderDeserializerConstructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, DbDataReaderMetadata.TypeArray);
            {
                var il = dataReaderDeserializerConstructor.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
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
                il.Emit(OpCodes.Callvirt, DbDataReaderMetadata.Dispose);
                il.Emit(OpCodes.Ret);
            }

            // MoveNext()
            {
                var method = typeBuilder.DefineMethod("MoveNext", MethodAttributes.Public | MethodAttributes.Virtual, typeof(bool), Type.EmptyTypes);
                var il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, readerField);
                il.Emit(OpCodes.Callvirt, DbDataReaderMetadata.Read);
                il.Emit(OpCodes.Ret);
            }

            // TReturn Deserialize<TReturn>(DBDataReader)
            var deserializeMethod = typeBuilder.DefineMethod(nameof(Deserialize), MethodAttributes.Public | MethodAttributes.Virtual);
            {
                var returnTypeParameter = deserializeMethod.DefineGenericParameters("TReturn")[0];

                deserializeMethod.SetReturnType(returnTypeParameter);
                deserializeMethod.SetParameters(DbDataReaderMetadata.TypeArray);

                var il = deserializeMethod.GetILGenerator();
                EmitDeserializeType(il, type, columnNames);
                il.Emit(OpCodes.Ret);
            }

            // IEnumerable<TReturn> DeserializeAll<TReturn>(DBDataReader)
            {
                var deserializeAllMethod = typeBuilder.DefineMethod(nameof(DeserializeAll), MethodAttributes.Public | MethodAttributes.Virtual);
                var returnTypeParameter = deserializeAllMethod.DefineGenericParameters("TReturn")[0];

                deserializeAllMethod.SetReturnType(typeof(IEnumerable<>).MakeGenericType(returnTypeParameter));
                deserializeAllMethod.SetParameters(DbDataReaderMetadata.TypeArray);

                var il = deserializeAllMethod.GetILGenerator();
                il.Emit(OpCodes.Ldarg_1);
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
                il.Emit(OpCodes.Callvirt, deserializeMethod.MakeGenericMethod(type));
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
                il.Emit(OpCodes.Callvirt, deserializeMethod.MakeGenericMethod(type));
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

        private static void EmitDeserializeType(ILGenerator il, Type type, string[] columnNames)
        {
            if (columnNames == null)
            {
                EmitDeserializeClrType(il, type);
            }
            else
            {
                EmitDeserializeUserType(il, type, columnNames);
            }
        }

        private static void EmitDeserializeClrType(ILGenerator il, Type type)
        {
            new DataReaderValueDeserializer(type, 0).EmitDeserializeClrType(il, false);
        }

        private static bool PropertyHasRequiredAttribute(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsClass == false)
            {
                return false;
            }

            using (var customAttributes = propertyInfo.GetCustomAttributes().GetEnumerator())
            {
                while (customAttributes.MoveNext())
                {
                    var type = customAttributes.Current.GetType();
                    if (type.Namespace == "System.ComponentModel.DataAnnotations" && type.Name == "RequiredAttribute")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void EmitDeserializeUserType(ILGenerator il, Type type, string[] columnNames)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                ThrowException.NoDefaultConstructor();
            }

            var sample = Activator.CreateInstance(type);

            il.Emit(OpCodes.Newobj, constructor);

            for (var ordinal = 0; ordinal < columnNames.Length; ordinal++)
            {
                var propertyInfo = type.GetProperty(columnNames[ordinal]);
                if (propertyInfo == null)
                {
                    continue;
                }

                if (propertyInfo.CanWrite && propertyInfo.GetSetMethod() != null)
                {
                    var propertyIsNullInitialized = propertyInfo.CanRead && propertyInfo.GetValue(sample) == null;
                    var hasRequiredAttribute = PropertyHasRequiredAttribute(propertyInfo);
                    var dataReaderPropertyValueDeserializer = new DataReaderValueDeserializer(propertyInfo, ordinal);
                    dataReaderPropertyValueDeserializer.EmitDeserializeProperty(il, propertyIsNullInitialized, hasRequiredAttribute);
                    continue;
                }

                var fieldInfo = type.GetField("<" + propertyInfo.Name + ">k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                if (fieldInfo != null)
                {
                    var fieldIsNullInitialized = fieldInfo.GetValue(sample) == null;
                    var hasRequiredAttribute = PropertyHasRequiredAttribute(propertyInfo);
                    var dataReaderFieldValueDeserializer = new DataReaderValueDeserializer(fieldInfo, ordinal);
                    dataReaderFieldValueDeserializer.EmitDeserializeProperty(il, fieldIsNullInitialized, hasRequiredAttribute);
                }
            }
        }
    }
}
