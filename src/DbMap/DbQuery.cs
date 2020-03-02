using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using DbMap.Deserialization;
using DbMap.Infrastructure;
using DbMap.Serialization;

namespace DbMap
{
    /// <summary>
    /// Represents a SQL statement to execute against a data source.
    /// </summary>
    public sealed class DbQuery
    {
        private readonly string sql;
        private readonly CommandType commandType;
        private readonly int? commandTimeout;

        private ParametersSerializer parametersSerializer;
        private DataReaderDeserializer dataReaderDeserializer;
        private ScalarDeserializer scalarDeserializer;

        private Type returnType;
        private Type parametersType;
        private Type dataParameterType;
        private Type connectionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQuery"/> class.
        /// </summary>
        /// <param name="sql"> SQL Statement. </param>
        /// <param name="commandType"> The command Type. </param>
        /// <param name="commandTimeout">Wait time in seconds before terminating the query.</param>
        public DbQuery(string sql, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            if (sql == null)
            {
                ThrowException.ValueCannotBeNull(nameof(sql));
            }

            this.commandType = commandType;
            this.sql = sql;
            this.commandTimeout = commandTimeout;
        }

        /// <summary>
        /// Execute query and returns the first column of the first row.
        /// </summary>
        /// <typeparam name="TReturn">Type of scalar value.</typeparam>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>Value of the first column of the first row.</returns>
        public TReturn ExecuteScalar<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, transaction, parameters))
            {
                if (ReferenceEquals(typeof(TReturn), returnType) == false)
                {
                    returnType = typeof(TReturn);
                    scalarDeserializer = ScalarDeserializerCache.GetCachedOrBuildNew(returnType);
                }

                var scalar = command.ExecuteScalar();
                return scalarDeserializer.Deserialize<TReturn>(scalar);
            }
        }

        /// <summary>
        /// Execute query and return all elements in the result.
        /// </summary>
        /// <typeparam name="TReturn">Type of value.</typeparam>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>All elements in the result.</returns>
        public IEnumerable<TReturn> ExecuteQuery<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, transaction, parameters))
            {
                var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess);
                SetupDataReaderDeserializer(typeof(TReturn), reader);

                return dataReaderDeserializer.DeserializeAll<TReturn>(reader);
            }
        }

        /// <summary>
        /// Execute query and return the first element in the result.
        /// </summary>
        /// <typeparam name="TReturn">Type of value.</typeparam>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>First element in the result.</returns>
        public TReturn ExecuteQueryFirst<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, transaction, parameters))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow | CommandBehavior.SequentialAccess))
            {
                SetupDataReaderDeserializer(typeof(TReturn), reader);
                return DbQueryInternal.QueryFirst<TReturn>(reader, dataReaderDeserializer);
            }
        }

        /// <summary>
        /// Execute query and return the first element in the result or a default value if the result is empty.
        /// </summary>
        /// <typeparam name="TReturn">Type of value.</typeparam>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>First element in the result or the default value.</returns>
        public TReturn ExecuteQueryFirstOrDefault<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, transaction, parameters))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow | CommandBehavior.SequentialAccess))
            {
                SetupDataReaderDeserializer(typeof(TReturn), reader);
                return DbQueryInternal.QueryFirstOrDefault<TReturn>(reader, dataReaderDeserializer);
            }
        }

        /// <summary>
        /// Execute query and return the only element in the result, and throws an exception if there is not exactly one element in the result.
        /// </summary>
        /// <typeparam name="TReturn">Type of value.</typeparam>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>Single element in the result.</returns>
        public TReturn ExecuteQuerySingle<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, transaction, parameters))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
            {
                SetupDataReaderDeserializer(typeof(TReturn), reader);
                return DbQueryInternal.QuerySingle<TReturn>(reader, dataReaderDeserializer);
            }
        }

        /// <summary>
        /// Execute query and return the only element in the result, or a default element if the result is empty; this method throws an exception if there is not exactly one element in the result.
        /// </summary>
        /// <typeparam name="TReturn">Type of value.</typeparam>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>Single element in the result or the default value.</returns>
        public TReturn ExecuteQuerySingleOrDefault<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, transaction, parameters))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
            {
                SetupDataReaderDeserializer(typeof(TReturn), reader);
                return DbQueryInternal.QuerySingleOrDefault<TReturn>(reader, dataReaderDeserializer);
            }
        }

        /// <summary>
        /// Execute query and return number of affected rows.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>Number of affected rows.</returns>
        public int Execute(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, transaction, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        private void SetupDataReaderDeserializer(Type type, DbDataReader reader)
        {
            if (ReferenceEquals(type, returnType)) 
            {
                return;
            }

            returnType = type;
            var columnNames = DbQueryInternal.IsClrType(returnType) ? null : DbQueryInternal.GetColumnNames(reader);
            dataReaderDeserializer = DataReaderDeserializerCache.GetCachedOrBuildNew(returnType, columnNames);
        }

        private IDbCommand SetupCommand(IDbConnection connection, IDbTransaction transaction, object parameters)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = commandType;

            if (commandTimeout.HasValue)
            {
                command.CommandTimeout = commandTimeout.Value;
            }

            command.Transaction = transaction;

            if (parameters == null)
            {
                return command;
            }

            var newParametersType = parameters.GetType();
            var newConnectionType = connection.GetType();

            if (ReferenceEquals(parametersType, newParametersType) == false || ReferenceEquals(connectionType, newConnectionType) == false)
            {
                connectionType = newConnectionType;
                parametersType = newParametersType;

                if (parametersType.IsClass == false)
                {
                    ThrowException.NotSupported();
                }

                dataParameterType = command.CreateParameter().GetType();
                parametersSerializer = ParametersSerializerCache.GetCachedOrBuildNew(dataParameterType, parametersType);
            }

            parametersSerializer.Serialize(command.Parameters, parameters);

            return command;
        }
    }
}