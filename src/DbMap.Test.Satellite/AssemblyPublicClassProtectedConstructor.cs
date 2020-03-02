namespace DbMap.Test.Assembly
{
    public class AssemblyPublicClassProtectedConstructor
    {
        public bool Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value == ((AssemblyPublicClassProtectedConstructor)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

    }
}