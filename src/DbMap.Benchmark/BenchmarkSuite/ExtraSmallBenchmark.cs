using System;
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
        private static readonly Func<DbMapDbContext, int, ExtraSmall> EFCoreLinqCompiledCompiled = EF.CompileQuery((DbMapDbContext context, int p1) => context.ExtraSmall.FirstOrDefault(extraSmall => p1 != 1));
        
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
        public ExtraSmall EFCoreLinqCompiledExtraSmall()
        {
            return EFCoreLinqCompiledCompiled(context, p1);
        }

        [Benchmark]
        public ExtraSmall EFCoreLinqExtraSmall()
        {
            return context.ExtraSmall.FirstOrDefault(extraSmall => p1 != 1);
        }

        [Benchmark]
        public ExtraSmall EFCoreInterpolatedExtraSmall()
        {
            return context.ExtraSmall.FromSqlInterpolated(SqlEFInterpolated).FirstOrDefault();
        }

        [Benchmark]
        public ExtraSmall EFCoreRawExtraSmall()
        {
            return context.ExtraSmall.FromSqlRaw(SqlEFRaw, ParametersArray).FirstOrDefault();
        }

        [Benchmark]
        public ExtraSmall DapperExtraSmall()
        {
            return connection.QueryFirstOrDefault<ExtraSmall>(Sql, Parameters);
        }

        [Benchmark]
        public ExtraSmall RepoDbExtraSmall()
        {
            return connection.ExecuteQuery<ExtraSmall>(Sql, Parameters).FirstOrDefault();
        }

        [Benchmark(Baseline = true)]
        public ExtraSmall DbMapExtraSmall()
        {
            return Query.QueryFirstOrDefault<ExtraSmall>(connection, Parameters);
        }
    }
}
