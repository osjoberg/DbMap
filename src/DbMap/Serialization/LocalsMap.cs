using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DbMap.Serialization
{
    internal class LocalsMap
    {
        private readonly ILGenerator il;
        private readonly List<Tuple<string, LocalBuilder>> locals = new List<Tuple<string, LocalBuilder>>(100);

        public LocalsMap(ILGenerator il)
        {     
            this.il = il;
        }

        public int DeclareLocal(Type type, string name)
        {
            var local = il.DeclareLocal(type);
            locals.Add(Tuple.Create(name, local));
            return local.LocalIndex;
        }

        public int? GetLocalIndex(Type type, string name)
        {
            for (var localIndex = 0; localIndex < locals.Count; localIndex++)
            {
                var (localName, local) = locals[localIndex];

                if (localName == name && ReferenceEquals(local.LocalType, type))
                {
                    return local.LocalIndex;
                }
            }

            return null;
        }
    }
}
