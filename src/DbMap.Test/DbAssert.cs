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

        public static void AreEqual(dynamic expected, Func<DbConnection, dynamic> func)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            ObjectAreEqual(expected, func(sqlConnection));
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

            var actualUserType = new DbQuery(query + " AS Value").QuerySingle<UserType<TReturn>>(sqlConnection).Value;
            Assert.AreEqual(expected, actualUserType);
        }

        public static void AreEqual<TReturn>(TReturn? expected, string sql) where TReturn : struct
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            var actualQuerySingle = new DbQuery(sql).QuerySingle<TReturn?>(sqlConnection);
            Assert.AreEqual(expected, actualQuerySingle);

            var actualQuery = new DbQuery(sql).Query<TReturn?>(sqlConnection).Single();
            Assert.AreEqual(expected, actualQuery);

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

        public static void CollectionAreEqual(IEnumerable<dynamic> expected, string sql)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            var actualQuery = new DbQuery(sql).Query(sqlConnection);

            var expectedArray = expected.ToArray();
            var actualArray = actualQuery.ToArray();

            CollectionAssert.AreEqual(expectedArray, actualArray);
        }

        public static void IsTrue(string sql, object parameters)
        {
            using var sqlConnection = new SqlConnection(ConnectionString);

            Assert.IsTrue(new DbQuery(sql).QuerySingle<bool>(sqlConnection, parameters));
        }

        private static void ObjectAreEqual(object expected, object actual)
        {
            if (expected == null && actual == null)
            {
                return;
            }

            if (expected == null || actual == null)
            {
                Assert.AreEqual(expected, actual);
            }

            var expectedProperties = expected.GetType().GetProperties();
            var actualProperties = actual.GetType().GetProperties();

            Assert.AreEqual(expectedProperties.Length, actualProperties.Length);

            foreach (var expectedProperty in expectedProperties)
            {
                var actualProperty = actualProperties.SingleOrDefault(property => property.Name == expectedProperty.Name);
                Assert.IsNotNull(actualProperty);

                var expectedPropertyValue = expectedProperty.GetValue(expected);
                var actualPropertyValue = actualProperty.GetValue(actual);

                Assert.AreEqual(expectedPropertyValue, actualPropertyValue, expectedProperty.Name);
            }
        }

        private class UserType<TReturn>
        {
            public TReturn Value { get; set; }
        }
    }
}
