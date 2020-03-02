using System;
using System.Data;
using System.Data.Common;

namespace DbMap.Benchmark.FakeProvider
{
    public class FakeParameter : DbParameter
    {
        private string parameterName;

        private object value;

        public FakeParameter()
        {
        }

        public FakeParameter(string parameterName, object value)
        {
            this.parameterName = parameterName;
            this.value = value;
        }

        public override DbType DbType { get; set; }

        public override ParameterDirection Direction { get; set; }

        public override bool IsNullable { get; set; }

        public override string ParameterName { get => parameterName; set => parameterName = value; }

        public override string SourceColumn { get; set; }

        public override object Value { get => value; set => this.value = value; }

        public override bool SourceColumnNullMapping { get; set; }

        public override int Size { get; set; }

        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }
    }
}
