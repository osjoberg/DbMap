namespace DbMap.Test.Assembly
{
    public class AssemblyInternalClass
    {
        public bool Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value == ((AssemblyInternalClass)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
