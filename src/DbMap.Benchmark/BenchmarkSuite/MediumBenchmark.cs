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
    public class MediumBenchmark
    {
        private static readonly Func<DbMapDbContext, int, int, int, int, int, IEnumerable<Medium>> EFCoreLinqCompiledCompiled = EF.CompileQuery((DbMapDbContext context, int p1, int p2, int p3, int p4, int p5) => context.Medium.Where(medium => p1 != p2 || p3 != p4 || p5 != 0));

        private static readonly int p1 = 1;
        private static readonly int p2 = 2;
        private static readonly int p3 = 3;
        private static readonly int p4 = 4;
        private static readonly int p5 = 5;

        private static readonly string Sql = $"SELECT {string.Join(", ", Medium.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM Medium WHERE @p1 <> @p2 OR @p3 <> @p4 OR @p5 <> 0";
        private static readonly string SqlEFRaw = Regex.Replace(Sql, "@p([0-9]+)", match => "{" + (int.Parse(match.Groups[1].Value) - 1) + "}");
        private static readonly FormattableString SqlEFInterpolated = $"SELECT [Boolean], [Decimal], [Double], [Int32], [String], [NullableBoolean], [NullableDecimal], [NullableDouble], [NullableInt32], [NullableString] FROM Medium WHERE {p1} <> {p2} OR {p3} <> {p4} OR {p5} <> 0";

        private static readonly object Parameters = new { p1, p2, p3, p4, p5 };
        private static readonly object[] ParametersArray = { p1, p2, p3, p4, p5 };
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
        public List<Medium> EFCoreLinqCompiledMedium()
        {
            return EFCoreLinqCompiledCompiled(context, p1, p2, p3, p4, p5).ToList();
        }

        [Benchmark]
        public List<Medium> EFCoreLinqMedium()
        {
            return context.Medium.Where(medium => p1 != p2 || p3 != p4 || p5 != 0).ToList();
        }

        [Benchmark]
        public List<Medium> EFCoreInterpolatedMedium()
        {
            return context.Medium.FromSqlInterpolated(SqlEFInterpolated).ToList();
        }

        [Benchmark]
        public List<Medium> EFCoreRawMedium()
        {
            return context.Medium.FromSqlRaw(SqlEFRaw, ParametersArray).ToList();
        }

        [Benchmark]
        public List<Medium> DapperMedium()
        {
            return connection.Query<Medium>(Sql, Parameters).AsList();
        }

        [Benchmark]
        public List<Medium> RepoDbMedium()
        {
            return connection.ExecuteQuery<Medium>(Sql, Parameters).ToList();
        }

        [Benchmark(Baseline = true)]
        public List<Medium> DbMapMedium()
        {
            return Query.Query<Medium>(connection, Parameters).ToList();
        }
    }
}
