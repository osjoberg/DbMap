namespace DbMap.Test.Deserialization
{
    public class NoPublicConstructor
    {
        private NoPublicConstructor()
        {
        }

        public int Value { get; set; }
    }
}
