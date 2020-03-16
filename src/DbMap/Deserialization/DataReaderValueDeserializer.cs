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

        public void EmitDeserializeProperty(ILGenerator il, bool isNullInitialized, bool hasRequiredAttribute)
        {
            if (isNullInitialized && hasRequiredAttribute == false)
            {
                EmitDeserializeNonNullValue(il);
                return;
            }

            EmitLoadUserType(il);
            EmitDeserializeClrType(il, hasRequiredAttribute);
            EmitAssignProperty(il);
        }

        public void EmitDeserializeClrType(ILGenerator il, bool hasRequiredAttribute)
        {
            if (hasRequiredAttribute || (nullableInfo == null && type.IsClass == false))
            {
                EmitGetValue(il);
            }
            else
            {
                EmitGetValueOrNull(il);
            }
        }

        private void EmitDeserializeNonNullValue(ILGenerator il)
        {
            var completeLabel = il.DefineLabel();

            il.Invoke(DbDataReaderMetadata.IsDBNull, ordinal);
            il.Emit(OpCodes.Brtrue_S, completeLabel);

            // Not null
            {
                EmitLoadUserType(il);
                EmitGetValue(il);
                EmitWrapNullable(il);
                EmitAssignProperty(il);
            }

            il.MarkLabel(completeLabel);
        }

        private void EmitLoadUserType(ILGenerator il)
        {
            il.Emit(OpCodes.Dup);
        }

        private void EmitGetValueOrNull(ILGenerator il)
        {
             var completeLabel = il.DefineLabel();

             EmitNull(il);
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

        private void EmitGetValue(ILGenerator il)
        {
            il.Invoke(getValueMethod, ordinal);
        }

        private void EmitNull(ILGenerator il)
        {
            if (nullableInfo != null)
            {
                il.Emit(OpCodes.Ldsfld, nullableInfo.NullConstant);
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