namespace DbMap.Test.Assembly
{
    public class AssemblyPublicClassInternalConstructor
    {
        public bool Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value == ((AssemblyPublicClassInternalConstructor)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}