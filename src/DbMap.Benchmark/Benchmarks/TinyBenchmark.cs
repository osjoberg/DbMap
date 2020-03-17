using System;
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
    public class TinyBenchmark
    {
        private static readonly int p1 = 1;

        private static readonly string Sql = $"SELECT {string.Join(", ", Tiny.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM Tiny WHERE @p1 = 1";
        private static readonly string SqlEFRaw = Regex.Replace(Sql, "@p([0-9]+)", match => "{" + (int.Parse(match.Groups[1].Value) - 1) + "}");
        private static readonly FormattableString SqlEFInterpolated = $"SELECT [String] FROM Tiny WHERE {p1} = 1";

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
        public string EFCoreLinqTiny()
        {
            return context.Tiny.Where(tiny => p1 == 1).Select(tiny => tiny.String).AsNoTracking().First();
        }

        [Benchmark]
        public string EFCoreInterpolatedTiny()
        {
            return context.Tiny.FromSqlInterpolated(SqlEFInterpolated).Select(tiny => tiny.String).AsNoTracking().First();
        }

        [Benchmark]
        public string EFCoreRawTiny()
        {
            return context.Tiny.FromSqlRaw(SqlEFRaw, ParametersArray).Select(tiny => tiny.String).AsNoTracking().First();
        }

        [Benchmark]
        public string DapperTiny()
        {
            return SqlMapper.ExecuteScalar<string>(connection, Sql, Parameters);
        }

        [Benchmark]
        public string RepoDbTiny()
        {
            return DbConnectionExtension.ExecuteScalar<string>(connection, Sql, Parameters);
        }

        [Benchmark(Baseline = true)]
        public string DbMapTiny()
        {
            return Query.ExecuteScalar<string>(connection, Parameters);
        }
    }
}
