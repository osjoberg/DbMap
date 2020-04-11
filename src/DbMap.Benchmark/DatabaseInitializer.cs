using System;
using System.Configuration;
using System.Linq;

using Upgrader;
using Upgrader.Schema;
using Upgrader.SqlServer;

namespace DbMap.Benchmark
{
    public static class DatabaseInitialize
    {
        public static void Initialize()
        {
            var steps = new StepCollection();

            steps.Add(
                "Create Tiny table",
                db =>
                    {
                        db.Tables.Add(
                            "Tiny",
                            new Column<string>("String", nullable: true, length: 10));

                        db.Tables["Tiny"].Rows.AddRange(Enumerable.Range(0, 1).Select(Tiny.Create));
                    });

            steps.Add(
                "Create Extra Small table",
                db =>
                {
                    db.Tables.Add(
                        "ExtraSmall",
                        new Column<bool>("Boolean"),
                        new Column<int>("Int32"),
                        new Column<string>("String", nullable: false, length: 10),
                        new Column<bool?>("NullableBoolean"),
                        new Column<int?>("NullableInt32"),
                        new Column<string>("NullableString", nullable: true, length: 10));

                    db.Tables["ExtraSmall"].Rows.AddRange(Enumerable.Range(0, 1).Select(ExtraSmall.Create));
                });

            steps.Add(
                "Create Small table",
                db =>
                    {
                        db.Tables.Add(
                            "Small",
                            new Column<bool>("Boolean"),
                            new Column<int>("Int32"),
                            new Column<string>("String", nullable: false, length: 10),
                            new Column<bool?>("NullableBoolean"),
                            new Column<int?>("NullableInt32"),
                            new Column<string>("NullableString", nullable: true, length: 10));

                        db.Tables["Small"].Rows.AddRange(Enumerable.Range(0, 10).Select(Small.Create));
                    });

            steps.Add(
                "Create Medium table",
                db =>
                {
                    db.Tables.Add(
                        "Medium",
                        new Column<bool>("Boolean"),
                        new Column<decimal>("Decimal"),
                        new Column<double>("Double"),
                        new Column<int>("Int32"),
                        new Column<string>("String", nullable: false, length: 10),
                        new Column<bool?>("NullableBoolean"),
                        new Column<decimal?>("NullableDecimal"),
                        new Column<double?>("NullableDouble"),
                        new Column<int?>("NullableInt32"),
                        new Column<string>("NullableString", nullable: true, length: 10));

                    db.Tables["Medium"].Rows.AddRange(Enumerable.Range(0, 100).Select(Medium.Create));
                });

            steps.Add(
                "Create Large table",
                db =>
                {
                    db.Tables.Add(
                        "Large",
                        new Column<bool>("Boolean"),
                        new Column<byte>("Byte"),
                        new Column<DateTime>("DateTime"),
                        new Column<decimal>("Decimal"),
                        new Column<double>("Double"),
                        new Column<Guid>("Guid"),
                        new Column<short>("Int16"),
                        new Column<int>("Int32"),
                        new Column<long>("Int64"),
                        new Column<float>("Single"),
                        new Column<string>("String", nullable: false, length: 10),
                        new Column<bool?>("NullableBoolean"),
                        new Column<byte?>("NullableByte"),
                        new Column<DateTime?>("NullableDateTime"),
                        new Column<decimal?>("NullableDecimal"),
                        new Column<double?>("NullableDouble"),
                        new Column<Guid?>("NullableGuid"),
                        new Column<short?>("NullableInt16"),
                        new Column<int?>("NullableInt32"),
                        new Column<long?>("NullableInt64"),
                        new Column<float?>("NullableSingle"),
                        new Column<string>("NullableString", nullable: true, length: 10));

                    db.Tables["Large"].Rows.AddRange(Enumerable.Range(0, 1000).Select(Large.Create));
                });

            steps.Add(
                "Create Extra Large table",
                db =>
                    {
                        db.Tables.Add(
                            "ExtraLarge",
                            new Column<bool>("Boolean"),
                            new Column<byte>("Byte"),
                            new Column<DateTime>("DateTime"),
                            new Column<decimal>("Decimal"),
                            new Column<double>("Double"),
                            new Column<Guid>("Guid"),
                            new Column<short>("Int16"),
                            new Column<int>("Int32"),
                            new Column<long>("Int64"),
                            new Column<float>("Single"),
                            new Column<string>("String", nullable: false, length: 10),
                            new Column<bool?>("NullableBoolean"),
                            new Column<byte?>("NullableByte"),
                            new Column<DateTime?>("NullableDateTime"),
                            new Column<decimal?>("NullableDecimal"),
                            new Column<double?>("NullableDouble"),
                            new Column<Guid?>("NullableGuid"),
                            new Column<short?>("NullableInt16"),
                            new Column<int?>("NullableInt32"),
                            new Column<long?>("NullableInt64"),
                            new Column<float?>("NullableSingle"),
                            new Column<string>("NullableString", nullable: true, length: 10));

                        db.Tables["ExtraLarge"].Rows.AddRange(Enumerable.Range(0, 10000).Select(Large.Create));
            });

            steps.Add(
                "Create String table",
                db =>
                {
                    db.Tables.Add("String", new Column<string>("Value", length: 50));
                    db.Tables["String"].Rows.AddRange(Enumerable.Range(1, 1000).Select(value => new { Value = value.ToString() }));
                });

            steps.Add(
                "Create Int32 table",
                db =>
                {
                    db.Tables.Add("Int32", new Column<int>("Value", ColumnModifier.PrimaryKey));
                    db.Tables["Int32"].Rows.AddRange(Enumerable.Range(1, 1000).Select(value => new { Value = value }));
                });

            steps.Add(
                "Create Multiply table",
                db =>
                {
                    db.Tables.Add("Multiply", new Column<int>("Value", ColumnModifier.PrimaryKey));
                    db.Tables["Multiply"].Rows.AddRange(Enumerable.Range(1, 1000).Select(value => new { Value = value }));
                });

            var connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
            var upgrader = new Upgrade<SqlServerDatabase>(connectionString);
            upgrader.PerformUpgrade(steps);
        }
    }
}
