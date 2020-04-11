using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using DbMap.Benchmark;
using DbMap.Benchmark.BenchmarkSuite;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbMap.Test
{
    [TestClass]
    public class BenchmarkSuiteTest
    {
        [TestMethod]
        [DynamicData(nameof(GetTinyBenchmarkResults), DynamicDataSourceType.Method)]
        public void TinyBenchmarkReturnsCorrectResult(string name, string item)
        {
            var expected = Tiny.Create(0).String;

            Assert.AreEqual(expected, item, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetExtraSmallBenchmarkResults), DynamicDataSourceType.Method)]
        public void ExtraSmallBenchmarkReturnsCorrectResult(string name, ExtraSmall item)
        {
            var expected = ExtraSmall.Create(0);

            Assert.AreEqual(expected, item, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetSmallBenchmarkResults), DynamicDataSourceType.Method)]
        public void SmallBenchmarkReturnsCorrectResult(string name, List<Small> list)
        {
            var expected = Enumerable.Range(0, 10).Select(Small.Create).ToList();

            CollectionAssert.AreEqual(expected, list, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetMediumBenchmarkResults), DynamicDataSourceType.Method)]
        public void MediumBenchmarkReturnsCorrectResult(string name, List<Medium> list)
        {
            var expected = Enumerable.Range(0, 100).Select(Medium.Create).ToList();

            CollectionAssert.AreEqual(expected, list, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetLargeBenchmarkResults), DynamicDataSourceType.Method)]
        public void LargeBenchmarkReturnsCorrectResult(string name, List<Large> list)
        {
            var expected = Enumerable.Range(0, 1000).Select(Large.Create).ToList();

            CollectionAssert.AreEqual(expected, list, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetExtraLargeBenchmarkResults), DynamicDataSourceType.Method)]
        public void ExtraLargeBenchmarkReturnsCorrectResult(string name, List<ExtraLarge> list)
        {
            var expected = Enumerable.Range(0, 10000).Select(ExtraLarge.Create).ToList();

            CollectionAssert.AreEqual(expected, list, name);
        }

        private static IEnumerable<object[]> GetTinyBenchmarkResults() => GetBenchmarkResults(typeof(TinyBenchmark));

        private static IEnumerable<object[]> GetExtraSmallBenchmarkResults() => GetBenchmarkResults(typeof(ExtraSmallBenchmark));

        private static IEnumerable<object[]> GetSmallBenchmarkResults() => GetBenchmarkResults(typeof(SmallBenchmark));

        private static IEnumerable<object[]> GetMediumBenchmarkResults() => GetBenchmarkResults(typeof(MediumBenchmark));

        private static IEnumerable<object[]> GetLargeBenchmarkResults() => GetBenchmarkResults(typeof(LargeBenchmark));

        private static IEnumerable<object[]> GetExtraLargeBenchmarkResults() => GetBenchmarkResults(typeof(ExtraLargeBenchmark));

        private static IEnumerable<object[]> GetBenchmarkResults(Type benchmarkType)
        {
            var benchmarkInstance = Activator.CreateInstance(benchmarkType);

            var globalSetup = benchmarkType.GetMethods().SingleOrDefault(method => method.GetCustomAttributes(typeof(GlobalSetupAttribute), false).Any());
            globalSetup?.Invoke(benchmarkInstance, null);

            var iterationSetup = benchmarkType.GetMethods().SingleOrDefault(method => method.GetCustomAttributes(typeof(IterationSetupAttribute), false).Any());
            iterationSetup?.Invoke(benchmarkInstance, null);

            return benchmarkType
                .GetMethods()
                .Where(method => method.GetCustomAttributes(typeof(BenchmarkAttribute), false).Any())
                .Select(method => new[] { method.Name, method.Invoke(benchmarkInstance, null) });
        }
    }
}
