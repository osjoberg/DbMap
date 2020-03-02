using System.Configuration;

using BenchmarkDotNet.Attributes;

using Dapper;

using Microsoft.Data.SqlClient;

using RepoDb;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 20, invocationCount: 1000)]
    public class ExecuteScalarBenchmark
    {
        private const string Int32Sql = "SELECT 1";
        private const string StringSql = "SELECT 'Test'";

        private static readonly DbQuery Int32Query = new DbQuery(Int32Sql);
        private static readonly DbQuery StringQuery = new DbQuery(StringSql);

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
            return SqlMapper.ExecuteScalar<int>(sqlConnection, Int32Sql);
        }

        [Benchmark]
        public int RepoDbInt32()
        {
            return DbConnectionExtension.ExecuteScalar<int>(sqlConnection, Int32Sql);
        }

        [Benchmark]
        public int DbQueryInt32()
        {
            return Int32Query.ExecuteScalar<int>(sqlConnection);
        }

        [Benchmark]
        public string DapperString()
        {
            return SqlMapper.ExecuteScalar<string>(sqlConnection, StringSql);
        }

        [Benchmark]
        public string RepoDbString()
        {
            return DbConnectionExtension.ExecuteScalar<string>(sqlConnection, StringSql);
        }

        [Benchmark]
        public string DbQueryString()
        {
            return StringQuery.ExecuteScalar<string>(sqlConnection);
        }
    }
}
