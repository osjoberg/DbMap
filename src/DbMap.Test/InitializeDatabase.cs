using DbMap.Benchmark;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test
{
    [TestClass]
    public class InitializeDatabase
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            DatabaseInitialize.Initialize();
        }
    }
}
