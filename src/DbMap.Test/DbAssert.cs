using System;
using System.Collections;
using System.Configuration;
using System.Data.Common;
using System.Linq;

using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test
{
    public static class DbAssert
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;

        public static void AreEqual<TReturn>(TReturn expected, Func<DbConnection, TReturn> func)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            Assert.AreEqual(expected, func(sqlConnection));
        }

        public static void AreEqual<TReturn>(TReturn expected, string query, bool isClrType = true)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            Assert.AreEqual(expected, new DbQuery(query).ExecuteQuerySingle<TReturn>(sqlConnection));
            Assert.AreEqual(expected, new DbQuery(query).ExecuteQuery<TReturn>(sqlConnection).Single());

            if (isClrType == false)
            {
                return;
            }

            Assert.AreEqual(expected, new DbQuery(query).ExecuteScalar<TReturn>(sqlConnection));
            Assert.AreEqual(expected, new DbQuery(query + " AS Value").ExecuteQuerySingle<Complex<TReturn>>(sqlConnection).Value);
        }

        public static void AreEqual<TReturn>(TReturn? expected, string sql) where TReturn: struct
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            Assert.AreEqual(expected, new DbQuery(sql).ExecuteQuerySingle<TReturn?>(sqlConnection));
            Assert.AreEqual(expected, new DbQuery(sql).ExecuteQuery<TReturn?>(sqlConnection).Single());
            Assert.AreEqual(expected, new DbQuery(sql).ExecuteScalar<TReturn?>(sqlConnection));
            Assert.AreEqual(expected, new DbQuery(sql + " AS Value").ExecuteQuerySingle<Complex<TReturn?>>(sqlConnection).Value);
        }

        public static void CollectionAreEqual<TReturn>(TReturn expected, string sql) where TReturn : ICollection
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            CollectionAssert.AreEqual(expected, new DbQuery(sql).ExecuteQuerySingle<TReturn>(sqlConnection));
            CollectionAssert.AreEqual(expected, new DbQuery(sql).ExecuteQuery<TReturn>(sqlConnection).Single());
            CollectionAssert.AreEqual(expected, new DbQuery(sql).ExecuteScalar<TReturn>(sqlConnection));
            CollectionAssert.AreEqual(expected, new DbQuery(sql + " AS Value").ExecuteQuerySingle<Complex<TReturn>>(sqlConnection).Value);
        }

        public static void IsTrue(string sql, object parameters)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            Assert.IsTrue(new DbQuery(sql).ExecuteQuerySingle<bool>(sqlConnection, parameters));
        }

        private class Complex<TReturn>
        {
            public TReturn Value { get; set; }
        }
    }
}
