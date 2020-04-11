using System.Data.Common;

namespace DbMap
{
    public abstract class ParametersSerializer
    {
        public abstract void Serialize(DbParameterCollection parameters, object @object);
    }
}