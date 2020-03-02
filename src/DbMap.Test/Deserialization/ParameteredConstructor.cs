namespace DbMap.Test.Deserialization
{
    public class ParameteredConstructor
    {
        public ParameteredConstructor(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }
}
