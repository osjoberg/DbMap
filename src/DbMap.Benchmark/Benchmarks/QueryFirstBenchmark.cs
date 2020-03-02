using System.Linq;

using BenchmarkDotNet.Attributes;

using Dapper;

using Microsoft.Data.SqlClient;

using RepoDb;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 20, invocationCount: 10000)]
    public abstract class QueryFirstBenchmark
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
            sqlConnection = new SqlConnection("Server=.;Database=DafTest;Integrated Security=SSPI;");
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            sqlConnection.Dispose();
        }

        [Benchmark]
        public int DapperInt32()
        {
            return sqlConnection.QueryFirst<int>(Int32Sql);
        }

        [Benchmark]
        public int DbQueryInt32()
        {
            return Int32Query.ExecuteQueryFirst<int>(sqlConnection);
        }

        [Benchmark]
        public string DapperString()
        {
            return sqlConnection.QueryFirst<string>(StringSql);
        }

        [Benchmark]
        public string DbQueryString()
        {
            return StringQuery.ExecuteQueryFirst<string>(sqlConnection);
        }

        [Benchmark]
        public Large DapperLarge()
        {
            return sqlConnection.QueryFirst<Large>(LargeSql);
        }

        [Benchmark]
        public Large RepoDbLarge()
        {
            return sqlConnection.ExecuteQuery<Large>(LargeSql).First();
        }

        [Benchmark]
        public Large DbQueryLarge()
        {
            return LargeQuery.ExecuteQueryFirst<Large>(sqlConnection);
        }
    }
}
