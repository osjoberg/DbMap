using BenchmarkDotNet.Attributes;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 3, warmupCount: 5, targetCount: 20, invocationCount: 1)]
    public class Query1MRowsBenchmark : QueryNRowsBenchmark
    {
        public Query1MRowsBenchmark() : base(1000000)
        {
        }
    }
}
