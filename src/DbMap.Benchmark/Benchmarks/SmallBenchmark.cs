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
    public class SmallBenchmark
    {
        private static readonly string SmallSql = $"SELECT {string.Join(", ", Small.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM Small";
        private static readonly object SmallParameters = new { p1 = 1 };
        private static readonly DbQuery SmallQuery = new DbQuery(SmallSql);

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
        public List<Small> EFCoreSmall()
        {
            return dbMapDbContext.Small.AsList();
        }

        [Benchmark]
        public List<Small> DapperSmall()
        {
            return sqlConnection.Query<Small>(SmallSql, SmallParameters).AsList();
        }

        [Benchmark]
        public List<Small> RepoDbSmall()
        {
            return sqlConnection.ExecuteQuery<Small>(SmallSql, SmallParameters).AsList();
        }

        [Benchmark]
        public List<Small> DbMapSmall()
        {
            return SmallQuery.Query<Small>(sqlConnection, SmallParameters).AsList();
        }
    }
}
