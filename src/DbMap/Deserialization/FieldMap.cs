using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DbMap.Deserialization
{
    internal class FieldMap
    {
        private readonly TypeBuilder typeBuilder;
        private readonly List<FieldBuilder> fields = new List<FieldBuilder>(100);

        public FieldMap(TypeBuilder typeBuilder)
        {
            this.typeBuilder = typeBuilder;
        }

        public FieldBuilder GetOrDeclareField(Type type, string fieldName)
        {
            FieldBuilder fieldBuilder;

            for (var fieldIndex = 0; fieldIndex < fields.Count; fieldIndex++)
            {
                fieldBuilder = fields[fieldIndex];

                if (fieldBuilder.Name == fieldName && ReferenceEquals(fieldBuilder.FieldType, type))
                {
                    return fieldBuilder;
                }
            }

            fieldBuilder = typeBuilder.DefineField(fieldName, type, FieldAttributes.HasDefault | FieldAttributes.InitOnly | FieldAttributes.Private | FieldAttributes.Static);
            fields.Add(fieldBuilder);
            return fieldBuilder;
        }
    }
}
