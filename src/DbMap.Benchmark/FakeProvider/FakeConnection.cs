using System;
using System.Data;
using System.Data.Common;

namespace DbMap.Benchmark.FakeProvider
{
    internal class FakeConnection : DbConnection
    {
        public override string ConnectionString { get; set; }

        public override string Database { get; } = null;

        public override ConnectionState State { get; } = ConnectionState.Closed;

        public override string DataSource { get; } = null;

        public override string ServerVersion { get; } = null;

        public override void Close()
        {
        }

        public override void Open()
        {
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand CreateDbCommand()
        {
            return new FakeCommand();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }
    }
}
