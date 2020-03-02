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
        private static readonly Type DbDataReaderType = typeof(DbDataReader);
        private static readonly Type[] DbDataReaderTypeArray = { DbDataReaderType };
        private static readonly MethodInfo DbDataReaderRead = DbDataReaderType.GetMethod(nameof(DbDataReader.Read));
        private static readonly MethodInfo DbDataReaderDispose = DbDataReaderType.GetMethod(nameof(DbDataReader.Dispose));
        private static readonly MethodInfo DbDataReaderIsDBNull = DbDataReaderType.GetMethod(nameof(DbDataReader.IsDBNull));
        private static readonly MethodInfo DbDataReaderGetBoolean = DbDataReaderType.GetMethod(nameof(DbDataReader.GetBoolean));
        private static readonly MethodInfo DbDataReaderGetByte = DbDataReaderType.GetMethod(nameof(DbDataReader.GetByte));
        private static readonly MethodInfo DbDataReaderGetChar = DbDataReaderType.GetMethod(nameof(DbDataReader.GetChar));
        private static readonly MethodInfo DbDataReaderGetDateTime = DbDataReaderType.GetMethod(nameof(DbDataReader.GetDateTime));
        private static readonly MethodInfo DbDataReaderGetDecimal = DbDataReaderType.GetMethod(nameof(DbDataReader.GetDecimal));
        private static readonly MethodInfo DbDataReaderGetDouble = DbDataReaderType.GetMethod(nameof(DbDataReader.GetDouble));
        private static readonly MethodInfo DbDataReaderGetInt16 = DbDataReaderType.GetMethod(nameof(DbDataReader.GetInt16));
        private static readonly MethodInfo DbDataReaderGetInt32 = DbDataReaderType.GetMethod(nameof(DbDataReader.GetInt32));
        private static readonly MethodInfo DbDataReaderGetInt64 = DbDataReaderType.GetMethod(nameof(DbDataReader.GetInt64));
        private static readonly MethodInfo DbDataReaderGetFloat = DbDataReaderType.GetMethod(nameof(DbDataReader.GetFloat));
        private static readonly MethodInfo DbDataReaderGetString = DbDataReaderType.GetMethod(nameof(DbDataReader.GetString));
        private static readonly MethodInfo DbDataReaderGetGuid = DbDataReaderType.GetMethod(nameof(DbDataReader.GetGuid));
        private static readonly MethodInfo DbDataReaderGetValue = DbDataReaderType.GetMethod(nameof(DbDataReader.GetValue));

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

            // Field.
            var readerField = typeBuilder.DefineField("reader", DbDataReaderType, FieldAttributes.Private);

            // Constructor.
            {
                var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
                var il = constructor.GetILGenerator();
                il.Emit(OpCodes.Ret);
            }

            // Constructor (IDataReader).
            var dataReaderConstructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, DbDataReaderTypeArray);
            {
                var il = dataReaderConstructor.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, readerField);
                il.Emit(OpCodes.Ret);
            }

            // Reset().
            {
                var method = typeBuilder.DefineMethod("Reset", MethodAttributes.Public | MethodAttributes.Virtual);
                var il = method.GetILGenerator();
                il.ThrowException(typeof(NotSupportedException));
                il.Emit(OpCodes.Ret);
            }

            // Dispose().
            {
                var method = typeBuilder.DefineMethod("Dispose", MethodAttributes.Public | MethodAttributes.Virtual);
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, readerField);
                il.Emit(OpCodes.Callvirt, DbDataReaderDispose);
                il.Emit(OpCodes.Ret);
            }

            // MoveNext().
            {
                var method = typeBuilder.DefineMethod("MoveNext", MethodAttributes.Public | MethodAttributes.Virtual, typeof(bool), Type.EmptyTypes);
                var il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, readerField);
                il.Emit(OpCodes.Callvirt, DbDataReaderRead);
                il.Emit(OpCodes.Ret);
            }

            // Deserialize method
            var deserializeMethod = typeBuilder.DefineMethod(nameof(Deserialize), MethodAttributes.Public | MethodAttributes.Virtual);
            {
                var returnTypeParameter = deserializeMethod.DefineGenericParameters("TReturn")[0];

                deserializeMethod.SetReturnType(returnTypeParameter);
                deserializeMethod.SetParameters(DbDataReaderTypeArray);

                var il = deserializeMethod.GetILGenerator();
                var locals = new LocalsMap(il);
                EmitDeserializeType(il, locals, type, columnNames);
                il.Emit(OpCodes.Ret);
            }

            // DeserializeAll method
            var deserializeAllMethod = typeBuilder.DefineMethod(nameof(DeserializeAll), MethodAttributes.Public | MethodAttributes.Virtual);
            {
                var returnTypeParameter = deserializeAllMethod.DefineGenericParameters("TReturn")[0];

                deserializeAllMethod.SetReturnType(typeof(IEnumerable<>).MakeGenericType(returnTypeParameter));
                deserializeAllMethod.SetParameters(DbDataReaderTypeArray);

                var il = deserializeAllMethod.GetILGenerator();
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Newobj, dataReaderConstructor);
                il.Emit(OpCodes.Ret);
            }

            // Current property (T).
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

            // Current property (object).
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

            // GetEnumerator method (IEnumerator<TReturn>)
            {
                var method = typeBuilder.DefineMethod("GetEnumerator", MethodAttributes.Public | MethodAttributes.Virtual, enumeratorType, Type.EmptyTypes);
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ret);
            }

            // GetEnumerator method (IEnumerator)
            {
                var method = typeBuilder.DefineMethod("GetEnumerator", MethodAttributes.Public | MethodAttributes.Virtual, typeof(IEnumerator), Type.EmptyTypes);
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, typeof(IEnumerator));
                il.Emit(OpCodes.Ret);
            }

            return (DataReaderDeserializer)Activator.CreateInstance(typeBuilder.CreateTypeInfo());
        }

        private static void EmitDeserializeType(ILGenerator il, LocalsMap locals, Type type, string[] columnNames)
        {
            if (columnNames == null)
            {
                EmitDeserializeClrType(il, locals, type);
            }
            else
            {
                EmitDeserializeUserType(il, locals, type, columnNames);
            }
        }

        private static void EmitDeserializeClrType(ILGenerator il, LocalsMap locals, Type type)
        {
            EmitGetValue(il, locals, type, 0);
        }

        private static void EmitDeserializeUserType(ILGenerator il, LocalsMap locals, Type type, string[] columnNames)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                ThrowException.NoDefaultConstructor();
            }

            var userTypeLocalIndex = locals.DeclareLocal(type, "userType");

            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Stloc, userTypeLocalIndex);

            for (var ordinal = 0; ordinal < columnNames.Length; ordinal++)
            {
                var propertyInfo = type.GetProperty(columnNames[ordinal]);
                if (propertyInfo == null)
                {
                    continue;
                }

                if (propertyInfo.CanWrite == false || propertyInfo.GetSetMethod() == null)
                {
                    var backingField = type.GetField("<" + propertyInfo.Name + ">k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (backingField == null)
                    {
                        continue;
                    }

                    il.Emit(OpCodes.Ldloc, userTypeLocalIndex);
                    EmitGetValue(il, locals, backingField.FieldType, ordinal);
                    il.Emit(OpCodes.Stfld, backingField);
                }
                else
                {
                    il.Emit(OpCodes.Ldloc, userTypeLocalIndex);
                    EmitGetValue(il, locals, propertyInfo.PropertyType, ordinal);
                    il.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                }
            }

            il.Emit(OpCodes.Ldloc, userTypeLocalIndex);
        }

        private static void EmitGetValue(ILGenerator il, LocalsMap locals, Type type, int ordinal)
        {
            var nullableInfo = NullableInfo.GetNullable(type);
            var underlyingType = nullableInfo?.UnderlyingType ?? type;

            var getValueMethod = GetGetValueMethodFromType(underlyingType);
            if (getValueMethod == null)
            {
                ThrowException.NotSupported();
            }

            if (nullableInfo != null)
            {
                var localIndex = locals.GetLocalIndex(type, "nullable") ?? locals.DeclareLocal(type, "nullable");
                EmitGetNullableValue(il, getValueMethod, ordinal, localIndex, nullableInfo);
            }
            else if (type.IsClass)
            {
                EmitGetReferenceValue(il, getValueMethod, ordinal);
            }
            else
            {
                EmitGetStackAllocatedValue(il, getValueMethod, ordinal);
            }
        }

        private static void EmitGetNullableValue(ILGenerator il, MethodInfo getValueMethod, int ordinal, int localIndex, NullableInfo nullableInfo)
        {
            var isNotDbNullLabel = il.DefineLabel();
            var completeLabel = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4_S, ordinal);
            il.Emit(OpCodes.Callvirt, DbDataReaderIsDBNull);
            il.Emit(OpCodes.Brfalse_S, isNotDbNullLabel);

            // Null
            {
                il.Emit(OpCodes.Ldloc_S, localIndex);
                il.Emit(OpCodes.Br_S, completeLabel);
            }

            // Not null
            {
                il.MarkLabel(isNotDbNullLabel);
                EmitGetStackAllocatedValue(il, getValueMethod, ordinal);
                il.Emit(OpCodes.Newobj, nullableInfo.Constructor);
            }

            il.MarkLabel(completeLabel);
        }

        private static void EmitGetReferenceValue(ILGenerator il, MethodInfo getValueMethod, int ordinal)
        {
            var isNotDbNullLabel = il.DefineLabel();
            var completeLabel = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4_S, ordinal);
            il.Emit(OpCodes.Callvirt, DbDataReaderIsDBNull);
            il.Emit(OpCodes.Brfalse_S, isNotDbNullLabel);

            // Null
            {
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Br_S, completeLabel);
            }

            // Not null
            {
                il.MarkLabel(isNotDbNullLabel);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4_S, ordinal);
                il.Emit(OpCodes.Callvirt, getValueMethod);
            }

            il.MarkLabel(completeLabel);
        }

        private static void EmitGetStackAllocatedValue(ILGenerator il, MethodInfo getValueMethod, int ordinal)
        {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4_S, ordinal);
            il.Emit(OpCodes.Callvirt, getValueMethod);
        }

        private static MethodInfo GetGetValueMethodFromType(Type underlyingType)
        {
            switch (Type.GetTypeCode(underlyingType))
            {
                case TypeCode.Boolean:
                    return DbDataReaderGetBoolean;

                case TypeCode.Byte:
                    return DbDataReaderGetByte;

                case TypeCode.Char:
                    return DbDataReaderGetChar;

                case TypeCode.DateTime:
                    return DbDataReaderGetDateTime;

                case TypeCode.Decimal:
                    return DbDataReaderGetDecimal;

                case TypeCode.Double:
                    return DbDataReaderGetDouble;

                case TypeCode.Int16:
                    return DbDataReaderGetInt16;

                case TypeCode.Int32:
                    return DbDataReaderGetInt32;

                case TypeCode.Int64:
                    return DbDataReaderGetInt64;

                case TypeCode.Single:
                    return DbDataReaderGetFloat;

                case TypeCode.String:
                    return DbDataReaderGetString;

                default:
                    if (underlyingType.IsArray)
                    {
                        return DbDataReaderGetValue;
                    }

                    if (ReferenceEquals(underlyingType, typeof(Guid)))
                    {
                        return DbDataReaderGetGuid;
                    }

                    return null;
            }
        }
    }
}
