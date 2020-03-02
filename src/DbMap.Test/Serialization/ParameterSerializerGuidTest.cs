using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test.Serialization
{
    [TestClass]
    public class ParameterSerializerGuidTest : ParameterSerializerTestBase<Guid>
    {
        public ParameterSerializerGuidTest() : base("SELECT CAST(IIF((@parameter IS NULL AND '{0}' = 'NULL') OR @parameter = '{0}', 1, 0) AS BIT)", Guid.Empty, Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"))
        {
        }
    }
}