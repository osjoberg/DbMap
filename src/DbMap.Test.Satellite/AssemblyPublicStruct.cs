namespace DbMap.Test.Assembly
{
    public struct AssemblyPublicStruct
    {
        public bool Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value == ((AssemblyPublicStruct)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

    }
}
