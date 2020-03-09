using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using DbMap.Benchmark.FakeProvider;

using RepoDb.Extensions;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 3, warmupCount: 5, targetCount: 20, invocationCount: 1000)]
    public class NullableBenchmark
    {
        private static readonly List<int?> NullValues = Enumerable.Repeat((int?)null, 100000).ToList();
        private static readonly List<int?> NotNullValues = Enumerable.Repeat((int?)45, 100000).ToList();
        private static readonly List<int?> EveryOtherNullValues = Enumerable.Range(0, 100000).Select(i => i % 2 == 0 ? null : (int?)45).ToList();

        private static readonly DbQuery NullQuery = new DbQuery("DbMap.Benchmark.Benchmarks.NullableBenchmark.NullMethod");
        private static readonly DbQuery NotNullQuery = new DbQuery("DbMap.Benchmark.Benchmarks.NullableBenchmark.NotNullMethod");
        private static readonly DbQuery EveryOtherNullQuery = new DbQuery("DbMap.Benchmark.Benchmarks.NullableBenchmark.EveryOtherNullMethod");

        private FakeConnection sqlConnection;

        public static List<int?> NullMethod()
        {
            return NullValues;
        }

        public static List<int?> NotNullMethod()
        {
            return NotNullValues;
        }

        public static List<int?> EveryOtherNullMethod()
        {
            return EveryOtherNullValues;
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
        public List<int?> Null()
        {
            return NullQuery.Query<int?>(sqlConnection).AsList();
        }

        [Benchmark]
        public List<int?> NotNull()
        {
            return NotNullQuery.Query<int?>(sqlConnection).AsList();
        }

        [Benchmark]
        public List<int?> EveryOtherNull()
        {
            return EveryOtherNullQuery.Query<int?>(sqlConnection).AsList();
        }
    }
}
