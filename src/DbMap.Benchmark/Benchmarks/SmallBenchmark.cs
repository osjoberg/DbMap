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

namespace DbMap.Benchmark.Benchmarks
{
    [SimpleJob(launchCount: 3, warmupCount: 5, targetCount: 20, invocationCount: 10000)]
    public class SmallBenchmark
    {
        private static readonly int p1 = 1;

        private static readonly string Sql = $"SELECT {string.Join(", ", Small.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM Small WHERE @p1 <> 1";
        private static readonly string SqlEFRaw = Regex.Replace(Sql, "@p([0-9]+)", match => "{" + (int.Parse(match.Groups[1].Value) - 1) + "}");
        private static readonly FormattableString SqlEFInterpolated = $"SELECT [Boolean], [Int32], [String], [NullableBoolean], [NullableInt32], [NullableString] FROM Small WHERE {p1} <> 1";

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
        public List<Small> EFCoreLinqSmall()
        {
            return context.Small.Where(small => p1 != 1).AsNoTracking().AsList();
        }

        [Benchmark]
        public List<Small> EFCoreRawSmall()
        {
            return context.Small.FromSqlRaw(SqlEFRaw, ParametersArray).AsNoTracking().AsList();
        }

        [Benchmark]
        public List<Small> EFCoreInterpolatedSmall()
        {
            return context.Small.FromSqlInterpolated(SqlEFInterpolated).AsNoTracking().AsList();
        }

        [Benchmark]
        public List<Small> DapperSmall()
        {
            return connection.Query<Small>(Sql, Parameters).AsList();
        }

        [Benchmark]
        public List<Small> RepoDbSmall()
        {
            return connection.ExecuteQuery<Small>(Sql, Parameters).AsList();
        }

        [Benchmark(Baseline = true)]
        public List<Small> DbMapSmall()
        {
            return Query.Query<Small>(connection, Parameters).AsList();
        }
    }
}
