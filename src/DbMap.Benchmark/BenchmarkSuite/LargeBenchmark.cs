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
    [SimpleJob(launchCount: 3, warmupCount: 5, targetCount: 20, invocationCount: 500)]
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
            return context.Large.Where(large => p1 != p2 || p3 != p4 || p5 != p6 || p7 != p8 || p9 != p10).ToList();
        }

        [Benchmark]
        public List<Large> EFCoreInterpolatedLarge()
        {
            return context.Large.FromSqlInterpolated(SqlEFInterpolated).ToList();
        }

        [Benchmark]
        public List<Large> EFCoreRawLarge()
        {
            return context.Large.FromSqlRaw(SqlEFRaw, ParametersArray).ToList();
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

        [Benchmark]
        public List<Large> HandwrittenLarge()
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
            command.Parameters.Add(new SqlParameter("p6", p6));
            command.Parameters.Add(new SqlParameter("p7", p7));
            command.Parameters.Add(new SqlParameter("p8", p8));
            command.Parameters.Add(new SqlParameter("p9", p9));
            command.Parameters.Add(new SqlParameter("p10", p10));

            using var reader = command.ExecuteReader();

            var result = new List<Large>();

            while (reader.Read())
            {
                var item = new Large
                {
                    Boolean = reader.GetBoolean(0),
                    Byte =  reader.GetByte(1),
                    DateTime = reader.GetDateTime(2),
                    Decimal = reader.GetDecimal(3),
                    Double = reader.GetDouble(4),
                    Guid = reader.GetGuid(5),
                    Int16 = reader.GetInt16(6),
                    Int32 = reader.GetInt32(7),
                    Int64 = reader.GetInt64(8),
                    Single = reader.GetFloat(9),
                    String = reader.GetString(10)
                };

                if (reader.IsDBNull(11) == false)
                {
                    item.NullableBoolean = reader.GetBoolean(11);
                }

                if (reader.IsDBNull(12) == false)
                {
                    item.NullableByte = reader.GetByte(12);
                }

                if (reader.IsDBNull(13) == false)
                {
                    item.NullableDateTime = reader.GetDateTime(13);
                }

                if (reader.IsDBNull(14) == false)
                {
                    item.NullableDecimal = reader.GetDecimal(14);
                }

                if (reader.IsDBNull(15) == false)
                {
                    item.NullableDouble = reader.GetDouble(15);
                }

                if (reader.IsDBNull(16) == false)
                {
                    item.NullableGuid = reader.GetGuid(16);
                }

                if (reader.IsDBNull(17) == false)
                {
                    item.NullableInt16 = reader.GetInt16(17);
                }

                if (reader.IsDBNull(18) == false)
                {
                    item.NullableInt32 = reader.GetInt32(18);
                }

                if (reader.IsDBNull(19) == false)
                {
                    item.NullableInt64 = reader.GetInt64(19);
                }

                if (reader.IsDBNull(20) == false)
                {
                    item.NullableSingle = reader.GetFloat(20);
                }

                if (reader.IsDBNull(21) == false)
                {
                    item.NullableString = reader.GetString(21);
                }

                result.Add(item);
            }

            return result;
        }
    }
}
