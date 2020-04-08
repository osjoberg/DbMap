using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

using BenchmarkDotNet.Attributes;

using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using RepoDb;

namespace DbMap.Benchmark.BenchmarkSuite
{
    [SimpleJob(launchCount: 3, warmupCount: 5, targetCount: 20, invocationCount: 5000)]
    public class MediumBenchmark
    {
        private static readonly Func<DbMapDbContext, int, int, int, int, int, IEnumerable<Medium>> EFCoreCompliedQuery = EF.CompileQuery((DbMapDbContext context, int p1, int p2, int p3, int p4, int p5) => context.Medium.Where(medium => p1 != p2 || p3 != p4 || p5 != 0));

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
        public List<Medium> EFCoreCompliedLinqMedium()
        {
            return EFCoreCompliedQuery(context, p1, p2, p3, p4, p5).ToList();
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


        [Benchmark]
        public List<Medium> HandwrittenMedium()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            using var command = new SqlCommand(Sql, connection);

            command.Parameters.Add(new SqlParameter("p1", p1));
            command.Parameters.Add(new SqlParameter("p2", p2));
            command.Parameters.Add(new SqlParameter("p3", p3));
            command.Parameters.Add(new SqlParameter("p4", p4));
            command.Parameters.Add(new SqlParameter("p5", p5));

            using var reader = command.ExecuteReader();

            var result = new List<Medium>();

            while (reader.Read())
            {
                var item = new Medium
                {
                   Boolean = reader.GetBoolean(0),
                   Decimal = reader.GetDecimal(1),
                   Double = reader.GetDouble(2),
                   Int32 = reader.GetInt32(3),
                   String = reader.GetString(4)
                };

                if (reader.IsDBNull(5) == false)
                {
                    item.NullableBoolean= reader.GetBoolean(5);
                }

                if (reader.IsDBNull(6) == false)
                {
                    item.NullableDecimal= reader.GetDecimal(6);
                }

                if (reader.IsDBNull(7) == false)
                {
                    item.NullableDouble = reader.GetDouble(7);
                }

                if (reader.IsDBNull(8) == false)
                {
                    item.NullableInt32 = reader.GetInt32(8);
                }

                if (reader.IsDBNull(9) == false)
                {
                    item.NullableString = reader.GetString(9);
                }

                result.Add(item);
            }

            return result;
        }
    }
}
