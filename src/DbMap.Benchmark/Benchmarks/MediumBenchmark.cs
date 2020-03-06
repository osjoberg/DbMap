using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using BenchmarkDotNet.Attributes;

using Dapper;

using Microsoft.Data.SqlClient;

using RepoDb;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 3, warmupCount: 5, targetCount: 20, invocationCount: 10000)]
    public class MediumBenchmark
    {
        private static readonly string MediumSql = $"SELECT {string.Join(", ", Medium.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM Medium";
        private static readonly object MediumParameters = new { p1 = 1, p2 = 2, p3 = 3, p4 = 4, p5 = 5 };
        private static readonly DbQuery MediumQuery = new DbQuery(MediumSql);

        private SqlConnection sqlConnection;
        private DbMapDbContext dbMapDbContext;

        [GlobalSetup]
        public void GlobalSetup()
        {
            SqlServerBootstrap.Initialize();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
            dbMapDbContext = new DbMapDbContext();
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            sqlConnection.Dispose();
            dbMapDbContext.Dispose();
        }

        [Benchmark]
        public List<Medium> EFCoreMedium()
        {
            return dbMapDbContext.Medium.AsList();
        }

        [Benchmark]
        public List<Medium> DapperMedium()
        {
            return sqlConnection.Query<Medium>(MediumSql, MediumParameters).AsList();
        }

        [Benchmark]
        public List<Medium> RepoDbMedium()
        {
            return sqlConnection.ExecuteQuery<Medium>(MediumSql, MediumParameters).AsList();
        }

        [Benchmark(Baseline = true)]
        public List<Medium> DbMapMedium()
        {
            return MediumQuery.Query<Medium>(sqlConnection, MediumParameters).AsList();
        }
    }
}
