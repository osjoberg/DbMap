using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using BenchmarkDotNet.Attributes;

using Dapper;

using Microsoft.Data.SqlClient;

using RepoDb;

namespace DbMap.Benchmark.Benchmarks
{
    public abstract class QueryNRowsBenchmark
    {
        private readonly string int32Sql;
        private readonly string stringSql;
        private readonly string largeSql;

        private readonly DbQuery int32Query;
        private readonly DbQuery stringQuery;
        private readonly DbQuery largeQuery;

        private SqlConnection sqlConnection;

        protected QueryNRowsBenchmark(int rowCount)
        {
            int32Sql = $"SELECT TOP {rowCount} Int32.Value FROM Int32";
            stringSql = $"SELECT TOP {rowCount} String.Value FROM String";
            largeSql = $"SELECT TOP {rowCount} {string.Join(", ", Large.GetAllPropertyNames().Select(name => "[" + name + "]"))} FROM Large";

            if (rowCount == 1000000)
            {
                int32Sql += " CROSS JOIN Multiply";
                stringSql += " CROSS JOIN Multiply";
                largeSql += " CROSS JOIN Multiply";
            }
            else if (rowCount > 1000000)
            {
                throw new NotSupportedException();
            }


            int32Query = new DbQuery(int32Sql);
            stringQuery = new DbQuery(stringSql);
            largeQuery = new DbQuery(largeSql);
        }

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
        public List<int> DapperInt32()
        {
            return sqlConnection.Query<int>(int32Sql).AsList();
        }

        [Benchmark]
        public List<int> DbQueryInt32()
        {
            return int32Query.ExecuteQuery<int>(sqlConnection).AsList();
        }

        [Benchmark]
        public List<string> DapperString()
        {
            return sqlConnection.Query<string>(stringSql).AsList();
        }

        [Benchmark]
        public List<string> DbQueryString()
        {
            return stringQuery.ExecuteQuery<string>(sqlConnection).AsList();
        }

        [Benchmark]
        public List<Large> DapperLarge()
        {
            return sqlConnection.Query<Large>(largeSql).AsList();
        }

        [Benchmark]
        public List<Large> RepoDbLarge()
        {
            return sqlConnection.ExecuteQuery<Large>(largeSql).AsList();
        }

        [Benchmark]
        public List<Large> DbMapLarge()
        {
            return largeQuery.ExecuteQuery<Large>(sqlConnection).AsList();
        }
    }
}
