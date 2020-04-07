using System;
using System.Reflection.Emit;

using DbMap.Infrastructure;

using static System.Reflection.Emit.OpCodes;

namespace DbMap.Deserialization
{
    internal static class TypeConverter
    {
        private static readonly OpCode[,] ConversionOpCodeTable =
        {
            /*                                                                                                                     FROM:                                                                                                                       */
            /* TO:                Empty  Object DBNull Boolean  Char            SByte        Byte            Int16        UInt16          Int32        UInt32          Int64        UInt64          Single       Double       Decimal DateTime <Undef.> String */
            /* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- */
            /* Empty    = 0  */ { Throw, Throw, Throw, Throw,   Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,       Throw,  Throw,   Throw,   Throw }, 
            /* Object   = 1  */ { Throw, Throw, Throw, Throw,   Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,       Throw,  Throw,   Throw,   Throw }, 
            /* DBNull   = 2  */ { Throw, Throw, Throw, Throw,   Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,       Throw,  Throw,   Throw,   Throw },
                                                                                                                                                                                                    
            /* Boolean  = 3  */ { Throw, Throw, Throw, Nop,     Cgt_Un,         Cgt_Un,      Cgt_Un,         Cgt_Un,      Cgt_Un,         Cgt_Un,      Cgt_Un,         Conv_I8,     Conv_I8,        Ldc_R4,      Ldc_R8,      Call,   Throw,   Throw,   Throw },
            /* Char     = 4  */ { Throw, Throw, Throw, Conv_U2, Nop,            Conv_Ovf_U2, Nop,            Conv_Ovf_U2, Nop,            Conv_Ovf_U2, Conv_Ovf_U2_Un, Conv_Ovf_U2, Conv_Ovf_U2_Un, Conv_Ovf_U2, Conv_Ovf_U2, Call,   Throw,   Throw,   Throw },  
            /* SByte    = 5  */ { Throw, Throw, Throw, Conv_I1, Conv_Ovf_I1_Un, Nop,         Conv_Ovf_I1_Un, Conv_Ovf_I1, Conv_Ovf_I1_Un, Conv_Ovf_I1, Conv_Ovf_I1_Un, Conv_Ovf_I1, Conv_Ovf_I1_Un, Conv_Ovf_I1, Conv_Ovf_I1, Call,   Throw,   Throw,   Throw },  
            /* Byte     = 6  */ { Throw, Throw, Throw, Conv_U1, Conv_Ovf_U1_Un, Conv_Ovf_U1, Nop,            Conv_Ovf_U1, Conv_Ovf_U1_Un, Conv_Ovf_U1, Conv_Ovf_U1_Un, Conv_Ovf_U1, Conv_Ovf_U1_Un, Conv_Ovf_U1, Conv_Ovf_U1, Call,   Throw,   Throw,   Throw },  
            /* Int16    = 7  */ { Throw, Throw, Throw, Conv_I2, Conv_Ovf_I2_Un, Nop,         Nop,            Nop,         Conv_Ovf_U1_Un, Conv_Ovf_I2, Conv_Ovf_U1_Un, Conv_Ovf_I2, Conv_Ovf_U1_Un, Conv_Ovf_I2, Conv_Ovf_I2, Call,   Throw,   Throw,   Throw },  
            /* UInt16   = 8  */ { Throw, Throw, Throw, Conv_U2, Nop,            Conv_Ovf_U2, Nop,            Conv_Ovf_U2, Nop,            Conv_Ovf_U2, Conv_Ovf_U1_Un, Conv_Ovf_U2, Conv_Ovf_U1_Un, Conv_Ovf_U2, Conv_Ovf_U2, Call,   Throw,   Throw,   Throw },  
            /* Int32    = 9  */ { Throw, Throw, Throw, Nop,     Nop,            Nop,         Nop,            Nop,         Nop,            Nop,         Conv_Ovf_I4_Un, Conv_Ovf_I4, Conv_Ovf_I4_Un, Conv_Ovf_I4, Conv_Ovf_I4, Call,   Throw,   Throw,   Throw },  
            /* UInt32   = 10 */ { Throw, Throw, Throw, Conv_U4, Nop,            Conv_Ovf_U4, Nop,            Conv_Ovf_U4, Nop,            Conv_Ovf_U4, Nop,            Conv_Ovf_U4, Conv_Ovf_U4_Un, Conv_Ovf_U4, Conv_Ovf_U4, Call,   Throw,   Throw,   Throw },  
            /* Int64    = 11 */ { Throw, Throw, Throw, Conv_I8, Conv_U8,        Conv_I8,     Conv_U8,        Conv_I8,     Conv_U8,        Conv_I8,     Conv_U8,        Nop,         Conv_Ovf_I8_Un, Conv_Ovf_I8, Conv_Ovf_I8, Call,   Throw,   Throw,   Throw },  
            /* UInt64   = 12 */ { Throw, Throw, Throw, Conv_U8, Conv_U8,        Conv_Ovf_U8, Conv_U8,        Conv_Ovf_U8, Conv_U8,        Conv_Ovf_U8, Conv_U8,        Conv_Ovf_U8, Nop,            Conv_Ovf_U8, Conv_Ovf_U8, Call,   Throw,   Throw,   Throw },  
            /* Single   = 13 */ { Throw, Throw, Throw, Conv_R4, Conv_R4,        Conv_R4,     Conv_R4,        Conv_R4,     Conv_R4,        Conv_R4,     Conv_R4,        Conv_R4,     Conv_R4,        Nop,         Conv_R4,     Call,   Throw,   Throw,   Throw },  
            /* Double   = 14 */ { Throw, Throw, Throw, Conv_R8, Conv_R8,        Conv_R8,     Conv_R8,        Conv_R8,     Conv_R8,        Conv_R8,     Conv_R8,        Conv_R8,     Conv_R8,        Conv_R8,     Nop,         Call,   Throw,   Throw,   Throw },
            /* Decimal  = 15 */ { Throw, Throw, Throw, Call,    Call,           Call,        Call,           Call,        Call,           Call,        Call,           Call,        Call,           Call,        Call,        Nop,    Throw,   Throw,   Throw }, 
                                                                                                                                                                                                   
            /* DateTime = 16 */ { Throw, Throw, Throw, Throw,  Throw,           Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,       Throw,  Nop,     Throw,   Throw }, 
            /* <Undef.> = 17 */ { Throw, Throw, Throw, Throw,  Throw,           Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,       Throw,  Throw,   Throw,   Throw },  
            /* String   = 18 */ { Throw, Throw, Throw, Throw,  Throw,           Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,          Throw,       Throw,       Throw,  Throw,   Throw,   Nop }
        };

        public static void EmitConversion(ILGenerator il, Type from, Type to)
        {
            if (from.IsClass == false && ReferenceEquals(to, typeof(object)))
            {
                il.Emit(OpCodes.Box, from);
                return;
            }

            var fromTypeCode = Type.GetTypeCode(from);
            var toTypeCode = Type.GetTypeCode(to);

            var conversionOpCode = ConversionOpCodeTable[(int)toTypeCode, (int)fromTypeCode];

            if (conversionOpCode == OpCodes.Nop)
            {
                return;
            }

            if (conversionOpCode == OpCodes.Throw)
            {
                // Guid cannot be resolved from ConversionOpCodeTable.
                if (ReferenceEquals(from, typeof(Guid)) && ReferenceEquals(to, typeof(Guid)))
                {
                    return;
                }

                // byte[] cannot be resolved from ConversionOpCodeTable.
                if (ReferenceEquals(from, typeof(object)) && ReferenceEquals(to, typeof(byte[])))
                {
                    return;
                }

                // char[] cannot be resolved from ConversionOpCodeTable.
                if (ReferenceEquals(from, typeof(object)) && ReferenceEquals(to, typeof(char[])))
                {
                    return;
                }

                il.Emit(OpCodes.Ldstr, ThrowException.InvalidCast(from, to));
                il.Emit(OpCodes.Newobj, typeof(InvalidCastException).GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Throw);
            }

            if (fromTypeCode == TypeCode.Boolean)
            {
                ConvertFromBoolean(il, conversionOpCode);
                return;
            }

            if (toTypeCode == TypeCode.Boolean)
            {
                ConvertToBoolean(il, conversionOpCode);
                return;
            }

            if (fromTypeCode == TypeCode.Decimal)
            {
                il.Emit(OpCodes.Call, DecimalMetadata.GetConversionMethodFromDecimal(toTypeCode));
                return;
            }

            if (toTypeCode == TypeCode.Decimal)
            {
                il.Emit(OpCodes.Call, DecimalMetadata.GetConversionMethodToDecimal(fromTypeCode));
                return;
            }

            if ((fromTypeCode == TypeCode.UInt32 || fromTypeCode == TypeCode.UInt64) && (toTypeCode == TypeCode.Single || toTypeCode == TypeCode.Double))
            {
                il.Emit(OpCodes.Conv_R_Un);
            }

            il.Emit(conversionOpCode);
        }

        private static void ConvertToBoolean(ILGenerator il, OpCode conversionOpCode)
        {
            if (conversionOpCode == OpCodes.Cgt_Un)
            {
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Cgt_Un);
            }
            else if (conversionOpCode == OpCodes.Conv_I8)
            {
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Conv_I8);
                il.Emit(OpCodes.Cgt_Un);
            }
            else if (conversionOpCode == OpCodes.Ldc_R4)
            {
                il.Emit(OpCodes.Ldc_R4, 0f);
                il.Emit(OpCodes.Ceq);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ceq);
            }
            else if (conversionOpCode == OpCodes.Ldc_R8)
            {
                il.Emit(OpCodes.Ldc_R8, 0d);
                il.Emit(OpCodes.Ceq);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ceq);
            }
            else if (conversionOpCode == OpCodes.Call)
            {
                il.Emit(OpCodes.Ldsfld, DecimalMetadata.ZeroField);
                il.Emit(OpCodes.Call, DecimalMetadata.OpInequalityMethod);
            }
        }

        private static void ConvertFromBoolean(ILGenerator il, OpCode conversionOpCode)
        {
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ceq);

            if (conversionOpCode == OpCodes.Call)
            {
                il.Emit(OpCodes.Call, DecimalMetadata.GetConversionMethodToDecimal(TypeCode.Int32));
            }
            else
            {
                il.Emit(conversionOpCode);
            }
        }
    }
}
