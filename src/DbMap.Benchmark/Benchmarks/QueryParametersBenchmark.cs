using System.Configuration;

using BenchmarkDotNet.Attributes;

using Dapper;

using Microsoft.Data.SqlClient;

using RepoDb;

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 20, invocationCount: 10000)]
    public class QueryParametersBenchmark
    {
        private const string NoParameterSql = "SELECT 1";
        private const string OneParameterSql = "SELECT @p1";
        private const string TenParametersSql = "SELECT @p1 + @p2 + @p3 + @p4 + @p5 + @p6 + @p7 + @p8 + @p9 + @p10";

        private static readonly DbQuery NoParameterQuery = new DbQuery(NoParameterSql);
        private static readonly DbQuery OneParameterQuery = new DbQuery(OneParameterSql);
        private static readonly DbQuery TenParametersQuery = new DbQuery(TenParametersSql);

        private static readonly object OneParameter = new { p1 = 1 };
        private static readonly object TenParameters = new { p1 = 1, p2 = 2, p3 = 3, p4 = 4, p5 = 5, p6 = 6, p7 = 7, p8 = 8, p9 = 9, p10 = 10 };

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
        public int DapperNoParameters()
        {
            return SqlMapper.ExecuteScalar<int>(sqlConnection, NoParameterSql);
        }

        [Benchmark]
        public int RepoDbNoParameters()
        {
            return DbConnectionExtension.ExecuteScalar<int>(sqlConnection, NoParameterSql);
        }

        [Benchmark]
        public int DbQueryNoParameters()
        {
            return NoParameterQuery.ExecuteScalar<int>(sqlConnection);
        }

        [Benchmark]
        public int DapperOneParameter()
        {
            return SqlMapper.ExecuteScalar<int>(sqlConnection, OneParameterSql, OneParameter);
        }

        [Benchmark]
        public int RepoDbOneParameter()
        {
            return DbConnectionExtension.ExecuteScalar<int>(sqlConnection, OneParameterSql, OneParameter);
        }

        [Benchmark]
        public int DbQueryOneParameter()
        {
            return OneParameterQuery.ExecuteScalar<int>(sqlConnection, OneParameter);
        }

        [Benchmark]
        public int DapperTenParameters()
        {
            return SqlMapper.ExecuteScalar<int>(sqlConnection, TenParametersSql, TenParameters);
        }

        [Benchmark]
        public int RepoDbTenParameters()
        {
            return DbConnectionExtension.ExecuteScalar<int>(sqlConnection, TenParametersSql, TenParameters);
        }

        [Benchmark]
        public int DbQueryTenParameters()
        {
            return TenParametersQuery.ExecuteScalar<int>(sqlConnection, TenParameters);
        }
    }
}
