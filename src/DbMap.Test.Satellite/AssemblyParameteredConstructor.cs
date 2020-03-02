namespace DbMap.Test.Assembly
{
    public class AssemblyParameteredConstructor
    {
        public AssemblyParameteredConstructor(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }
}
