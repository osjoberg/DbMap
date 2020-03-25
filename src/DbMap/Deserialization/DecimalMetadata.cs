using System;
using System.Reflection;

namespace DbMap.Deserialization
{
    public static class DecimalMetadata
    {
        public static readonly FieldInfo ZeroField = typeof(decimal).GetField("Zero");
        public static readonly MethodInfo OpInequalityMethod = typeof(decimal).GetMethod("op_Inequality");

        private static readonly MethodInfo[] ConvertToDecimalMethods = new MethodInfo[19];
        private static readonly MethodInfo[] ConvertFromDecimalMethods = new MethodInfo[19];

        static DecimalMetadata()
        {
            var methods = typeof(decimal).GetMethods();

            for (var index = 0; index < methods.Length; index++)
            {
                var method = methods[index];
                
                if (method.Name == "op_Explicit")
                {
                    if (ReferenceEquals(method.ReturnType, typeof(decimal)))
                    {
                        var fromTypeIndex = (int)Type.GetTypeCode(method.GetParameters()[0].ParameterType);
                        ConvertToDecimalMethods[fromTypeIndex] = method;
                    }
                    else
                    {
                        var toTypeIndex = (int)Type.GetTypeCode(method.ReturnType);
                        ConvertFromDecimalMethods[toTypeIndex] = method;
                    }
                }
                else if (method.Name == "op_Implicit")
                {
                    var fromTypeIndex = (int)Type.GetTypeCode(method.GetParameters()[0].ParameterType);
                    
                    if (ConvertToDecimalMethods[fromTypeIndex] == null)
                    {
                        ConvertToDecimalMethods[fromTypeIndex] = method;
                    }
                }
            }
        }

        public static MethodInfo GetConversionMethodFromDecimal(TypeCode to)
        {
            return ConvertFromDecimalMethods[(int)to];
        }

        public static MethodInfo GetConversionMethodToDecimal(TypeCode from)
        {
            return ConvertToDecimalMethods[(int)from];
        }
    }
}
