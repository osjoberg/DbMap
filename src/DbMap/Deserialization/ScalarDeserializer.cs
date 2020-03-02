using System;
using System.Reflection;
using System.Reflection.Emit;

using DbMap.Infrastructure;

namespace DbMap.Deserialization
{
    public abstract class ScalarDeserializer
    {
        private static readonly FieldInfo DbNull = typeof(DBNull).GetField(nameof(DBNull.Value));

        public abstract TReturn Deserialize<TReturn>(object value);

        internal static ScalarDeserializer Create(Type type)
        {
            var moduleBuilder = DynamicAssembly.GetExistingDynamicAssemblyOrCreateNew(type.Assembly);

            var typeName = DynamicAssembly.GetUniqueTypeName("Daf.Map.Runtime." + type.Name + "ScalarDeserializer");
            var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, typeof(ScalarDeserializer));

            // Deserialize().
            {
                var method = typeBuilder.DefineMethod(nameof(Deserialize), MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard);

                var returnTypeParameter = method.DefineGenericParameters("TReturn")[0];
                method.SetReturnType(returnTypeParameter);
                method.SetParameters(typeof(object));

                var il = method.GetILGenerator();

                var nullableInfo = NullableInfo.GetNullable(type);
                if (nullableInfo != null)
                {
                    EmitCastNullableValue(il, nullableInfo);
                }
                else if (type.IsClass)
                {
                    EmitCastReferenceValue(il);
                }
                else
                {
                    EmitCastStackAllocatedValue(il, type);
                }
            }

            return (ScalarDeserializer)Activator.CreateInstance(typeBuilder.CreateTypeInfo());
        }

        private static void EmitCastNullableValue(ILGenerator il, NullableInfo nullableInfo)
        {
            var isDbNullLabel = il.DefineLabel();

            // No result.
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Beq_S, isDbNullLabel);

            // Null result.
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldsfld, DbNull);
            il.Emit(OpCodes.Beq_S, isDbNullLabel);

            // Not null
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Unbox_Any, nullableInfo.UnderlyingType);
                il.Emit(OpCodes.Newobj, nullableInfo.Constructor);
                il.Emit(OpCodes.Ret);
            }

            // Null
            {
                il.MarkLabel(isDbNullLabel);
                il.Emit(OpCodes.Ldsfld, nullableInfo.NullConstant);
                il.Emit(OpCodes.Ret);
            }

            il.Emit(OpCodes.Ret);
        }

        private static void EmitCastReferenceValue(ILGenerator il)
        {
            var isDbNullLabel = il.DefineLabel();

            // No result.
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Beq_S, isDbNullLabel);

            // Null result.
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldsfld, DbNull);
            il.Emit(OpCodes.Beq_S, isDbNullLabel);

            // Not null
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ret);
            }

            // Null
            {
                il.MarkLabel(isDbNullLabel);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ret);
            }
        }

        private static void EmitCastStackAllocatedValue(ILGenerator il, Type type)
        {
            var isDbNullLabel = il.DefineLabel();

            il.DeclareLocal(type);

            // No result.
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Beq_S, isDbNullLabel);

            // Null result.
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldsfld, DbNull);
            il.Emit(OpCodes.Beq_S, isDbNullLabel);

            // Not null
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Ret);
            }

            // Null
            {
                il.MarkLabel(isDbNullLabel);
                il.Emit(OpCodes.Ldloca_S, 0);
                il.Emit(OpCodes.Initobj, type);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ret);
            }
        }
    }
}
