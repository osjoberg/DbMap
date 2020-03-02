namespace DbMap.Test.Deserialization
{
    public class PublicClassInternalConstructor
    {
        public bool Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value == ((PublicClassInternalConstructor)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}