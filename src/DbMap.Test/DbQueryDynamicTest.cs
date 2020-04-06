using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test
{
    [TestClass]
    public class DbQueryDynamicTest
    {
        [TestMethod]
        public void ExecuteQueryFirst()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column]");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QueryFirst(connection, new { parameter1 = 1 }));
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Sequence contains no elements")]
        public void ExecuteQueryFirstThrowsOnNoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] WHERE 1 = 0");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QueryFirst(connection, new { parameter1 = 1 }));
        }

        [TestMethod]
        public void ExecuteQueryFirstReturnsFirstRowOnTwoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] UNION SELECT @parameter2 AS [Column]");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QueryFirst(connection, new { parameter1 = 1, parameter2 = 2 }));
        }

        [TestMethod]
        public void ExecuteQueryFirstOrDefault()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column]");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QueryFirstOrDefault(connection, new { parameter1 = 1 }));
        }

        [TestMethod]
        public void ExecuteQueryFirstOrDefaultReturnsNullOnNoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] WHERE 1 = 0");
            DbAssert.AreEqual(null, connection => query.QueryFirstOrDefault(connection, new { parameter1 = 1 }));
        }

        [TestMethod]
        public void ExecuteQueryFirstOrDefaultReturnsDefaultOnNoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] WHERE 1 = 0");
            DbAssert.AreEqual(null, connection => query.QueryFirstOrDefault(connection, new { parameter1 = 1 }));
        }

        [TestMethod]
        public void ExecuteQueryFirstOrDefaultReturnsFirstRowOnTwoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] UNION SELECT @parameter2 AS [Column] ");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QueryFirstOrDefault(connection, new { parameter1 = 1, parameter2 = 2 }));
        }

        [TestMethod]
        public void ExecuteQuerySingle()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column]");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QuerySingle(connection, new { parameter1 = 1 }));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Sequence contains no elements")]
        public void ExecuteQuerySingleThrowsOnNoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] WHERE 1 = 0");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QuerySingle(connection, new { parameter1 = 1 }));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Sequence contains more than one element")]
        public void ExecuteQuerySingleThrowsOnTwoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] UNION SELECT @parameter2 AS [Column] ");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QuerySingle(connection, new { parameter1 = 1, parameter2 = 2 }));
        }

        [TestMethod]
        public void ExecuteQuerySingleOrDefault()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column]");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QuerySingleOrDefault(connection, new { parameter1 = 1 }));
        }

        [TestMethod]
        public void ExecuteQuerySingleOrDefaultReturnsNullOnNoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] WHERE 1 = 0");
            DbAssert.AreEqual(null, connection => query.QuerySingleOrDefault(connection, new { parameter1 = 1 }));
        }

        [TestMethod]
        public void ExecuteQuerySingleOrDefaultReturnsDefaultOnNoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] WHERE 1 = 0");
            DbAssert.AreEqual(null, connection => query.QuerySingleOrDefault(connection, new { parameter1 = 1 }));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Sequence contains more than one element")]
        public void ExecuteQuerySingleOrDefaultThrowsOnTwoRows()
        {
            var query = new DbQuery("SELECT @parameter1 AS [Column] UNION SELECT @parameter2 AS [Column] ");
            DbAssert.AreEqual(new { Column = 1 }, connection => query.QuerySingleOrDefault(connection, new { parameter1 = 1, parameter2 = 2 }));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Field name 'Column' is not unique which is required to be able to materialize type.")]
        public void ExecuteQuerySingleOrDefaultThrowsOnDuplicateColumnNames()
        {
            var query = new DbQuery("SELECT 1 AS [Column], 2 AS [Column]");
            DbAssert.AreEqual(1, connection => query.QueryFirst(connection));
        }
    }
}