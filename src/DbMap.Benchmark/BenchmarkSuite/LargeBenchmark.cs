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
    [SimpleJob(launchCount: 3, warmupCount: 5, targetCount: 20, invocationCount: 1000)]
    public class LargeBenchmark
    {
        private static readonly int p1 = 1;
        private static readonly int p2 = 2;
        private static readonly int p3 = 3;
        private static readonly int p4 = 4;
        private static readonly int p5 = 5;
        private static readonly int p6 = 6;
        private static readonly int p7 = 7;
        private static readonly int p8 = 8;
        private static readonly int p9 = 9;
        private static readonly int p10 = 10;

        private static readonly string Sql = $"SELECT {string.Join(", ", Large.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM Large WHERE @p1 <> @p2 OR @p3 <> @p4 OR @p5 <> @p6 OR @p7 <> @p8 OR @p9 <> @p10";
        private static readonly string SqlEFRaw = Regex.Replace(Sql, "@p([0-9]+)", match => "{" + (int.Parse(match.Groups[1].Value) - 1) + "}");
        private static readonly FormattableString SqlEFInterpolated = $"SELECT [Boolean], [Byte], [DateTime], [Decimal], [Double], [Guid], [Int16], [Int32], [Int64], [Single], [String], [NullableBoolean], [NullableByte], [NullableDateTime], [NullableDecimal], [NullableDouble], [NullableGuid], [NullableInt16], [NullableInt32], [NullableInt64], [NullableSingle], [NullableString] FROM Large WHERE {p1} <> {p2} OR {p3} <> {p4} OR {p5} <> {p6} OR {p7} <> {p8} OR {p9} <> {p10}";
        private static readonly object Parameters = new { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 };
        private static readonly object[] ParametersArray = { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 };
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
        public List<Large> EFCoreLinqLarge()
        {
            return context.Large.Where(large => p1 != p2 || p3 != p4 || p5 != p6 || p7 != p8 || p9 != p10).AsNoTracking().ToList();
        }

        [Benchmark]
        public List<Large> EFCoreInterpolatedLarge()
        {
            return context.Large.FromSqlInterpolated(SqlEFInterpolated).AsNoTracking().ToList();
        }

        [Benchmark]
        public List<Large> EFCoreRawLarge()
        {
            return context.Large.FromSqlRaw(SqlEFRaw, ParametersArray).AsNoTracking().ToList();
        }

        [Benchmark]
        public List<Large> DapperLarge()
        {
            return connection.Query<Large>(Sql, Parameters).AsList();
        }

        [Benchmark]
        public List<Large> RepoDbLarge()
        {
            return connection.ExecuteQuery<Large>(Sql, Parameters).ToList();
        }

        [Benchmark(Baseline = true)]
        public List<Large> DbMapLarge()
        {
            return Query.Query<Large>(connection, Parameters).ToList();
        }
    }
}
