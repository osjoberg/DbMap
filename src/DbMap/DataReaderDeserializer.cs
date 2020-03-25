using System.Collections.Generic;
using System.Data.Common;

namespace DbMap
{
    public abstract class DataReaderDeserializer
    {
        public abstract TReturn Deserialize<TReturn>(DbDataReader reader);

        public abstract IEnumerable<TReturn> DeserializeAll<TReturn>(DbCommand command, DbDataReader reader);
    }
}
