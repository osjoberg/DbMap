using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using BenchmarkDotNet.Attributes;

using Dapper;

using Microsoft.Data.SqlClient;

using RepoDb;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 3, warmupCount: 5, targetCount: 20, invocationCount: 100)]
    public class LargeBenchmark
    {
        private static readonly string LargeSql = $"SELECT {string.Join(", ", Large.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM Large";
        private static readonly object LargeParameters = new { p1 = 1, p2 = 2, p3 = 3, p4 = 4, p5 = 5, p6 = 6, p7 = 7, p8 = 8, p9 = 9, p10 = 10 };
        private static readonly DbQuery LargeQuery = new DbQuery(LargeSql);

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
        public List<Large> EFCoreLarge()
        {
            return dbMapDbContext.Large.AsList();
        }

        [Benchmark]
        public List<Large> DapperLarge()
        {
            return sqlConnection.Query<Large>(LargeSql, LargeParameters).AsList();
        }

        [Benchmark]
        public List<Large> RepoDbLarge()
        {
            return sqlConnection.ExecuteQuery<Large>(LargeSql, LargeParameters).AsList();
        }

        [Benchmark]
        public List<Large> DbMapLarge()
        {
            return LargeQuery.ExecuteQuery<Large>(sqlConnection, LargeParameters).AsList();
        }
    }
}
