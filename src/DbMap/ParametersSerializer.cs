using System.Collections;

namespace DbMap
{
    public abstract class ParametersSerializer
    {
        public abstract void Serialize(IList parameters, object @object);
    }
}