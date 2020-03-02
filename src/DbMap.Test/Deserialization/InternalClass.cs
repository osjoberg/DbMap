namespace DbMap.Test.Deserialization
{
    public class InternalClass
    {
        public bool Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value == ((InternalClass)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}