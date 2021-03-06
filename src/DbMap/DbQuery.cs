﻿using System;
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
        internal static readonly string[] NoColumnNames = new string[0];

        private readonly string sql;
        private readonly CommandType commandType;
        private readonly int? commandTimeout;

        private ParametersSerializer parametersSerializer;
        private DataReaderDeserializer dataReaderDeserializer;

        private Type returnType;
        private Type parametersType;
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

            using (var command = SetupCommand(connection, parameters, transaction))
            {
                return command.ExecuteNonQuery();
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
        public IEnumerable<TReturn> Query<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            var command = (DbCommand)SetupCommand(connection, parameters, transaction);
            var reader = command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess);
            SetupDataReaderDeserializer(typeof(TReturn), reader);

            return dataReaderDeserializer.DeserializeAll<TReturn>(command, reader);
        }

        /// <summary>
        /// Execute query and return all elements in the result.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>All elements in the result.</returns>
        public IEnumerable<dynamic> Query(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            var command = (DbCommand)SetupCommand(connection, parameters, transaction);
            var reader = command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess);
            SetupDataReaderDeserializer(reader);

            return dataReaderDeserializer.DeserializeAll<dynamic>(command, reader);
        }

        /// <summary>
        /// Execute query and return the first element in the result.
        /// </summary>
        /// <typeparam name="TReturn">Type of value.</typeparam>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>First element in the result.</returns>
        public TReturn QueryFirst<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, parameters, transaction))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess | CommandBehavior.SingleRow))
            {
                SetupDataReaderDeserializer(typeof(TReturn), reader);
                return DbQueryInternal.QueryFirst<TReturn>(reader, dataReaderDeserializer);
            }
        }

        /// <summary>
        /// Execute query and return the first element in the result.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>First element in the result.</returns>
        public dynamic QueryFirst(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, parameters, transaction))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess | CommandBehavior.SingleRow))
            {
                SetupDataReaderDeserializer(reader);
                return DbQueryInternal.QueryFirst<dynamic>(reader, dataReaderDeserializer);
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
        public TReturn QueryFirstOrDefault<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, parameters, transaction))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess | CommandBehavior.SingleRow))
            {
                SetupDataReaderDeserializer(typeof(TReturn), reader);
                return DbQueryInternal.QueryFirstOrDefault<TReturn>(reader, dataReaderDeserializer);
            }
        }

        /// <summary>
        /// Execute query and return the first element in the result or a default value if the result is empty.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>First element in the result or the default value.</returns>
        public dynamic QueryFirstOrDefault(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }
            
            using (var command = SetupCommand(connection, parameters, transaction))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess | CommandBehavior.SingleRow))
            {
                SetupDataReaderDeserializer(reader);
                return DbQueryInternal.QueryFirstOrDefault<dynamic>(reader, dataReaderDeserializer);
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
        public TReturn QuerySingle<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, parameters, transaction))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
            {
                SetupDataReaderDeserializer(typeof(TReturn), reader);
                return DbQueryInternal.QuerySingle<TReturn>(reader, dataReaderDeserializer);
            }
        }

        /// <summary>
        /// Execute query and return the only element in the result, and throws an exception if there is not exactly one element in the result.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>Single element in the result.</returns>
        public dynamic QuerySingle(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, parameters, transaction))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
            {
                SetupDataReaderDeserializer(reader);
                return DbQueryInternal.QuerySingle<dynamic>(reader, dataReaderDeserializer);
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
        public TReturn QuerySingleOrDefault<TReturn>(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, parameters, transaction))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
            {
                SetupDataReaderDeserializer(typeof(TReturn), reader);
                return DbQueryInternal.QuerySingleOrDefault<TReturn>(reader, dataReaderDeserializer);
            }
        }

        /// <summary>
        /// Execute query and return the only element in the result, or a default element if the result is empty; this method throws an exception if there is not exactly one element in the result.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>Single element in the result or the default value.</returns>
        public dynamic QuerySingleOrDefault(IDbConnection connection, object parameters = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                ThrowException.ValueCannotBeNull(nameof(connection));
            }

            using (var command = SetupCommand(connection, parameters, transaction))
            using (var reader = (DbDataReader)command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
            {
                SetupDataReaderDeserializer(reader);
                return DbQueryInternal.QuerySingleOrDefault<dynamic>(reader, dataReaderDeserializer);
            }
        }

        private IDbCommand SetupCommand(IDbConnection connection, object parameters, IDbTransaction transaction)
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

            var currentConnectionType = connection.GetType();

            if (parameters == null)
            {
                if (ReferenceEquals(connectionType, currentConnectionType) == false)
                {
                    connectionType = currentConnectionType;
                    returnType = null;
                }

                return command;
            }

            var currentParametersType = parameters.GetType();

            if (ReferenceEquals(parametersType, currentParametersType) == false || ReferenceEquals(connectionType, currentConnectionType) == false)
            {
                connectionType = currentConnectionType;
                parametersType = currentParametersType;
                returnType = null;

                if (parametersType.IsClass == false)
                {
                    ThrowException.NotSupported();
                }

                parametersSerializer = ParametersSerializerCache.GetCachedOrBuildNew(connectionType, parametersType);
            }

            parametersSerializer.Serialize((DbParameterCollection)command.Parameters, parameters);

            return command;
        }

        private void SetupDataReaderDeserializer(Type type, DbDataReader reader)
        {
            if (ReferenceEquals(type, returnType)) 
            {
                return;
            }

            var columnNames = DbQueryInternal.IsClrType(type) ? NoColumnNames : DbQueryInternal.GetColumnNames(reader);
            var columnTypes = DbQueryInternal.GetColumnTypes(reader);
            dataReaderDeserializer = DataReaderDeserializerCache.GetCachedOrBuildNew(connectionType, type, columnNames, columnTypes);
            returnType = type;
        }

        private void SetupDataReaderDeserializer(DbDataReader reader)
        {
            if (returnType != null)
            {
                return;
            }

            var columnNames = DbQueryInternal.GetColumnNames(reader);
            var columnTypes = DbQueryInternal.GetColumnTypes(reader);

            returnType = DynamicTypeCache.GetCachedOrBuildNew(columnNames, columnTypes);

            dataReaderDeserializer = DataReaderDeserializerCache.GetCachedOrBuildNew(connectionType, returnType, columnNames, columnTypes);
        }
    }
}