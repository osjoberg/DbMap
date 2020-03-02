using BenchmarkDotNet.Attributes;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 20, invocationCount: 1000)]
    public class Query1KRowsBenchmark : QueryNRowsBenchmark
    {
        public Query1KRowsBenchmark() : base(1000)
        {
        }
    }
}
