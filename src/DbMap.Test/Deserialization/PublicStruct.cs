using System;

namespace DbMap.Test.Deserialization
{
    public struct PublicStruct
    {
        public bool Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value == ((PublicStruct)obj).Value;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
