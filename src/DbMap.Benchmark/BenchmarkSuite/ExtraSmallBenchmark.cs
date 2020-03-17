using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

using BenchmarkDotNet.Attributes;

using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using RepoDb;

namespace DbMap.Benchmark.BenchmarkSuite
{
    [SimpleJob(launchCount: 3, warmupCount: 5, targetCount: 20, invocationCount: 10000)]
    public class ExtraSmallBenchmark
    {
        private static readonly int p1 = 1;

        private static readonly string Sql = $"SELECT {string.Join(", ", ExtraSmall.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM ExtraSmall WHERE @p1 <> 1";
        private static readonly string SqlEFRaw = Regex.Replace(Sql, "@p([0-9]+)", match => "{" + (int.Parse(match.Groups[1].Value) - 1) + "}");
        private static readonly FormattableString SqlEFInterpolated = $"SELECT [Boolean], [Int32], [String], [NullableBoolean], [NullableInt32], [NullableString] FROM ExtraSmall WHERE {p1} <> 1";

        private static readonly object Parameters = new { p1 };
        private static readonly object[] ParametersArray = { p1 };
        private static readonly DbQuery Query = new DbQuery(Sql);

        private SqlConnection connection;
        private DbMapDbContext context;

        [GlobalSetup]
        public void GlobalSetup()
        {
            SqlServerBootstrap.Initialize();

            var indexOfFirstParameter = Sql.IndexOf("@", StringComparison.Ordinal);
            if (Sql.Substring(0, indexOfFirstParameter) != SqlEFInterpolated.ToString().Substring(0, indexOfFirstParameter))
            {
                throw new Exception();
            }
        }

        [IterationSetup]
        public void IterationSetup()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
            context = new DbMapDbContext();
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            connection.Dispose();
            context.Dispose();
        }

        [Benchmark]
        public List<ExtraSmall> EFCoreLinqExtraSmall()
        {
            return context.ExtraSmall.Where(extraSmall => p1 != 1).AsNoTracking().AsList();
        }

        [Benchmark]
        public List<ExtraSmall> EFCoreInterpolatedExtraSmall()
        {
            return context.ExtraSmall.FromSqlInterpolated(SqlEFInterpolated).AsNoTracking().AsList();
        }

        [Benchmark]
        public List<ExtraSmall> EFCoreRawExtraSmall()
        {
            return context.ExtraSmall.FromSqlRaw(SqlEFRaw, ParametersArray).AsNoTracking().AsList();
        }

        [Benchmark]
        public List<ExtraSmall> DapperExtraSmall()
        {
            return connection.Query<ExtraSmall>(Sql, Parameters).AsList();
        }

        [Benchmark]
        public List<ExtraSmall> RepoDbExtraSmall()
        {
            return connection.ExecuteQuery<ExtraSmall>(Sql, Parameters).AsList();
        }

        [Benchmark(Baseline = true)]
        public List<ExtraSmall> DbMapExtraSmall()
        {
            return Query.Query<ExtraSmall>(connection, Parameters).AsList();
        }
    }
}
