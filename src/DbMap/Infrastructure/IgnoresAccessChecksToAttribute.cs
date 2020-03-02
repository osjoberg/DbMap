namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class IgnoresAccessChecksToAttribute : Attribute
    {
        public IgnoresAccessChecksToAttribute(string assemblyName)
        {
        }
    }
}