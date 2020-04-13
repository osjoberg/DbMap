using System;
using System.Diagnostics;
using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using Dapper;

using DbMap.Benchmark.BenchmarkSuite;

using Microsoft.EntityFrameworkCore;

using RepoDb;

namespace DbMap.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DatabaseInitialize.Initialize();

            Run<TinyBenchmark>();
            Run<ExtraSmallBenchmark>();
            Run<SmallBenchmark>();
            Run<MediumBenchmark>();
            Run<LargeBenchmark>();
            Run<ExtraLargeBenchmark>();

            Console.WriteLine("EF Core v" + typeof(DbContext).Assembly.GetName().Version);
            Console.WriteLine("Dapper v" + typeof(SqlMapper).Assembly.GetName().Version);
            Console.WriteLine("RepoDb v" + typeof(DbConnectionExtension).Assembly.GetName().Version);
            Console.WriteLine("DBMap v" + typeof(DbQuery).Assembly.GetName().Version);

            Console.ReadLine();
        }

        private static void Run<T>()
        {
            if (Debugger.IsAttached == false)
            {
                BenchmarkRunner.Run<T>();
                return;
            }

            const int iterations = 10000;

            var instance = Activator.CreateInstance<T>();

            var globalSetupMethod = typeof(T).GetMethods().SingleOrDefault(method => method.GetCustomAttributes(typeof(GlobalSetupAttribute), false).Any());
            if (globalSetupMethod != null)
            {
                globalSetupMethod.Invoke(instance, null);
            }

            var iterationSetupMethod = typeof(T).GetMethods().SingleOrDefault(method => method.GetCustomAttributes(typeof(IterationSetupAttribute), false).Any());
            if (iterationSetupMethod != null)
            {
                iterationSetupMethod.Invoke(instance, null);
            }

            foreach (var benchmarkMethod in typeof(T).GetMethods().Where(method => method.GetCustomAttributes(typeof(BenchmarkAttribute), false).Any()).ToArray())
            {
                for (var iteration = 0; iteration < iterations; iteration++)
                {
                    benchmarkMethod.Invoke(instance, null);
                }
            }

            var iterationCleanupMethod = typeof(T).GetMethods().SingleOrDefault(method => method.GetCustomAttributes(typeof(IterationCleanupAttribute), false).Any());
            if (iterationCleanupMethod != null)
            {
                iterationCleanupMethod.Invoke(instance, null);
            }
        }
    }
}
