using System;
using System.Collections;
using System.Collections.Generic;
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

            var actualQuerySingle = new DbQuery(query).QuerySingle<TReturn>(sqlConnection);
            Assert.AreEqual(expected, actualQuerySingle);

            var actualQuery = new DbQuery(query).Query<TReturn>(sqlConnection).Single();
            Assert.AreEqual(expected, actualQuery);

            if (isClrType == false)
            {
                return;
            }

            var actualScalar = new DbQuery(query).ExecuteScalar<TReturn>(sqlConnection);
            Assert.AreEqual(expected, actualScalar);

            var actualUserType = new DbQuery(query + " AS Value").QuerySingle<UserType<TReturn>>(sqlConnection).Value;
            Assert.AreEqual(expected, actualUserType);
        }

        public static void AreEqual<TReturn>(TReturn? expected, string sql) where TReturn: struct
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            var actualQuerySingle = new DbQuery(sql).QuerySingle<TReturn?>(sqlConnection);
            Assert.AreEqual(expected, actualQuerySingle);

            var actualQuery = new DbQuery(sql).Query<TReturn?>(sqlConnection).Single();
            Assert.AreEqual(expected, actualQuery);

            var actualScalar = new DbQuery(sql).ExecuteScalar<TReturn?>(sqlConnection);
            Assert.AreEqual(expected, actualScalar);

            var actualUserType = new DbQuery(sql + " AS Value").QuerySingle<UserType<TReturn?>>(sqlConnection).Value;
            Assert.AreEqual(expected, actualUserType);
        }

        public static void ArrayAreEqual<TReturn>(TReturn expected, string sql) where TReturn : ICollection
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            var actualQuerySingle = new DbQuery(sql).QuerySingle<TReturn>(sqlConnection);
            CollectionAssert.AreEqual(expected, actualQuerySingle);

            var actualQuery = new DbQuery(sql).Query<TReturn>(sqlConnection).Single();
            CollectionAssert.AreEqual(expected, actualQuery);

            var actualScalar = new DbQuery(sql).ExecuteScalar<TReturn>(sqlConnection);
            CollectionAssert.AreEqual(expected, actualScalar);

            var actualUserType = new DbQuery(sql + " AS Value").QuerySingle<UserType<TReturn>>(sqlConnection).Value;
            CollectionAssert.AreEqual(expected, actualUserType);
        }

        public static void CollectionAreEqual<TReturn>(IEnumerable<TReturn> expected, string sql)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            var actualQuery = new DbQuery(sql).Query<TReturn>(sqlConnection);

            var expectedArray = expected.ToArray();
            var actualArray = actualQuery.ToArray();

            CollectionAssert.AreEqual(expectedArray, actualArray);
        }

        public static void IsTrue(string sql, object parameters)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            Assert.IsTrue(new DbQuery(sql).QuerySingle<bool>(sqlConnection, parameters));
        }

        private class UserType<TReturn>
        {
            public TReturn Value { get; set; }
        }
    }
}
