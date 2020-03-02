using BenchmarkDotNet.Attributes;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 20, invocationCount: 10000)]
    public class Query10RowsBenchmark : QueryNRowsBenchmark
    {
        public Query10RowsBenchmark() : base(10)
        {
        }
    }
}
