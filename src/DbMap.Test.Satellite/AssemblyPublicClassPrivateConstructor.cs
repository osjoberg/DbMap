namespace DbMap.Test.Assembly
{
    public class AssemblyPublicClassPrivateConstructor
    {
        public bool Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value == ((AssemblyPublicClassPrivateConstructor)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}