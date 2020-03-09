using System;
using System.Reflection;
using System.Reflection.Emit;
using DbMap.Infrastructure;

namespace DbMap.Deserialization
{
    internal class DataReaderValueDeserializer
    {
        private readonly Type type;
        private readonly int ordinal;
        private readonly MethodInfo getValueMethod;
        private readonly NullableInfo nullableInfo;
        private readonly FieldInfo fieldInfo;
        private readonly PropertyInfo propertyInfo;

        public DataReaderValueDeserializer(Type type, int ordinal)
        {
            this.type = type;
            this.ordinal = ordinal;

            nullableInfo = NullableInfo.GetNullable(type);

            getValueMethod = DbDataReaderMetadata.GetGetValueMethodFromType(nullableInfo?.UnderlyingType ?? type);
            if (getValueMethod == null)
            {
                ThrowException.NotSupported();
            }
        }

        public DataReaderValueDeserializer(FieldInfo fieldInfo, int ordinal) : this(fieldInfo.FieldType, ordinal)
        {
            this.fieldInfo = fieldInfo;
        }

        public DataReaderValueDeserializer(PropertyInfo propertyInfo, int ordinal) : this(propertyInfo.PropertyType, ordinal)
        {
            this.propertyInfo = propertyInfo;
        }

        public void EmitDeserializeProperty(ILGenerator il, LocalsMap locals, bool isNullInitialized)
        {
            if (isNullInitialized)
            {
                EmitDeserializeNonNullValue(il);
                return;
            }

            EmitLoadUserType(il);
            EmitDeserializeClrType(il, locals);
            EmitAssignProperty(il);
        }

        public void EmitDeserializeClrType(ILGenerator il, LocalsMap locals)
        {
            if (nullableInfo != null || type.IsClass)
            {
                EmitGetValueOrNull(il, locals);
            }
            else
            {
                EmitGetValue(il);
            }
        }

        private static void EmitLoadUserType(ILGenerator il)
        {
            il.Emit(OpCodes.Dup);
        }

        private void EmitGetValue(ILGenerator il)
        {
            il.Invoke(getValueMethod, ordinal);
        }

        private void EmitDeserializeNonNullValue(ILGenerator il)
        {
            var completeLabel = il.DefineLabel();

            il.Invoke(DbDataReaderMetadata.IsDBNull, ordinal);
            il.Emit(OpCodes.Brtrue_S, completeLabel);

            // Not null
            {
                il.Emit(OpCodes.Dup);
                il.Invoke(getValueMethod, ordinal);
                EmitWrapNullable(il);
                EmitAssignProperty(il);
            }

            il.MarkLabel(completeLabel);
        }

        private void EmitGetValueOrNull(ILGenerator il, LocalsMap locals)
        {
             var completeLabel = il.DefineLabel();

             EmitNull(il, locals);
             il.Invoke(DbDataReaderMetadata.IsDBNull, ordinal);
             il.Emit(OpCodes.Brtrue_S, completeLabel);

             // Not null
             {
                 il.Emit(OpCodes.Pop);
                 il.Invoke(getValueMethod, ordinal);
                 EmitWrapNullable(il);
             }

             il.MarkLabel(completeLabel);
        }

        private void EmitNull(ILGenerator il, LocalsMap locals)
        {
            if (nullableInfo != null)
            {
                var localIndex = locals.GetLocalIndex(type, "nullable") ?? locals.DeclareLocal(type, "nullable");
                il.Emit(OpCodes.Ldloc_S, localIndex);
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
        }

        private void EmitWrapNullable(ILGenerator il)
        {
            if (nullableInfo != null)
            {
                il.Emit(OpCodes.Newobj, nullableInfo.Constructor);
            }
        }

        private void EmitAssignProperty(ILGenerator il)
        {
            if (fieldInfo != null)
            {
                il.Emit(OpCodes.Stfld, fieldInfo);
            }
            else if (propertyInfo != null)
            {
                il.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
            }
        }
    }
}