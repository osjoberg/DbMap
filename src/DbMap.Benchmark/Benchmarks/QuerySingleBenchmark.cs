using System.Configuration;
using System.Linq;

using BenchmarkDotNet.Attributes;

using Dapper;

using Microsoft.Data.SqlClient;

using RepoDb;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 20, invocationCount: 10000)]
    public abstract class QuerySingleBenchmark
    {
        private const string Int32Sql = "SELECT TOP 1 Value FROM Int32";
        private const string StringSql = "SELECT TOP 1 Value FROM String";
        private static readonly string LargeSql = $"SELECT TOP 1 {string.Join(", ", Large.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM Large";

        private static readonly DbQuery Int32Query = new DbQuery(Int32Sql);
        private static readonly DbQuery StringQuery = new DbQuery(StringSql);
        private static readonly DbQuery LargeQuery = new DbQuery(LargeSql);

        private SqlConnection sqlConnection;

        [GlobalSetup]
        public void GlobalSetup()
        {
            SqlServerBootstrap.Initialize();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            sqlConnection.Dispose();
        }

        [Benchmark]
        public int DapperInt32()
        {
            return sqlConnection.QuerySingle<int>(Int32Sql);
        }

        [Benchmark]
        public int DbQueryInt32()
        {
            return Int32Query.QuerySingle<int>(sqlConnection);
        }

        [Benchmark]
        public string DapperString()
        {
            return sqlConnection.QuerySingle<string>(StringSql);
        }

        [Benchmark]
        public string DbQueryString()
        {
            return StringQuery.QuerySingle<string>(sqlConnection);
        }

        [Benchmark]
        public Large DapperLarge()
        {
            return sqlConnection.QuerySingle<Large>(LargeSql);
        }

        [Benchmark]
        public Large RepoDbLarge()
        {
            return sqlConnection.ExecuteQuery<Large>(LargeSql).Single();
        }

        [Benchmark]
        public Large DbMapLarge()
        {
            return LargeQuery.QuerySingle<Large>(sqlConnection);
        }
    }
}
