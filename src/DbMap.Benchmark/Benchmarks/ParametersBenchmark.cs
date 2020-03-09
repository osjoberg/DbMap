using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using DbMap.Benchmark.FakeProvider;

using RepoDb.Extensions;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 20, invocationCount: 1000000)]
    public class ParametersBenchmark
    {
        private static readonly List<int> Values = Enumerable.Repeat(1, 1).ToList();

        private static readonly DbQuery Query = new DbQuery("DbMap.Benchmark.Benchmarks.ParametersBenchmark.ValuesMethod");

        private static readonly object Int32NullParameters = new { p0 = (int?)null };
        private static readonly object Int32ValueParameters = new { p0 = 1 };
        private static readonly object StringNullParameters = new { p0 = (string)null };
        private static readonly object StringValueParameters = new { p0 = "Test" };

        private FakeConnection sqlConnection;

        public static List<int> ValuesMethod()
        {
            return Values;
        }

        [IterationSetup]
        public void IterationSetup()
        {
            sqlConnection = new FakeConnection();
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            sqlConnection.Dispose();
        }

        [Benchmark]
        public List<int?> Int32Null()
        {
            return Query.Query<int?>(sqlConnection, Int32NullParameters).AsList();
        }

        [Benchmark]
        public List<int?> Int32NotNull()
        {
            return Query.Query<int?>(sqlConnection, Int32ValueParameters).AsList();
        }

        [Benchmark]
        public List<int?> StringNull()
        {
            return Query.Query<int?>(sqlConnection, StringNullParameters).AsList();
        }

        [Benchmark]
        public List<int?> StringNotNull()
        {
            return Query.Query<int?>(sqlConnection, StringValueParameters).AsList();
        }
    }
}
