namespace DbMap.Test.Deserialization
{
    public class PublicClassPrivateConstructor
    {
        public bool Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value == ((PublicClassPrivateConstructor)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}