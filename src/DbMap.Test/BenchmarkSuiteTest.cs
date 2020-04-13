using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public void TinyBenchmarkReturnsCorrectResult(string name, MethodInfo method, object instance)
        {
            var expected = Tiny.Create(0).String;
            var actual = (string)method.Invoke(instance, null);

            Assert.AreEqual(expected, actual, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetExtraSmallBenchmarkResults), DynamicDataSourceType.Method)]
        public void ExtraSmallBenchmarkReturnsCorrectResult(string name, MethodInfo method, object instance)
        {
            var expected = ExtraSmall.Create(0);
            var actual = (ExtraSmall)method.Invoke(instance, null);

            Assert.AreEqual(expected, actual, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetSmallBenchmarkResults), DynamicDataSourceType.Method)]
        public void SmallBenchmarkReturnsCorrectResult(string name, MethodInfo method, object instance)
        {
            var expected = Enumerable.Range(0, 10).Select(Small.Create).ToList();
            var actual = (List<Small>)method.Invoke(instance, null);

            CollectionAssert.AreEqual(expected, actual, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetMediumBenchmarkResults), DynamicDataSourceType.Method)]
        public void MediumBenchmarkReturnsCorrectResult(string name, MethodInfo method, object instance)
        {
            var expected = Enumerable.Range(0, 100).Select(Medium.Create).ToList();
            var actual = (List<Medium>)method.Invoke(instance, null);

            CollectionAssert.AreEqual(expected, actual, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetLargeBenchmarkResults), DynamicDataSourceType.Method)]
        public void LargeBenchmarkReturnsCorrectResult(string name, MethodInfo method, object instance)
        {
            var expected = Enumerable.Range(0, 1000).Select(Large.Create).ToList();
            var actual = (List<Large>)method.Invoke(instance, null);

            CollectionAssert.AreEqual(expected, actual, name);
        }

        [TestMethod]
        [DynamicData(nameof(GetExtraLargeBenchmarkResults), DynamicDataSourceType.Method)]
        public void ExtraLargeBenchmarkReturnsCorrectResult(string name, MethodInfo method, object instance)
        {
            var expected = Enumerable.Range(0, 10000).Select(ExtraLarge.Create).ToList();
            var actual = (List<ExtraLarge>)method.Invoke(instance, null);

            CollectionAssert.AreEqual(expected, actual, name);
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
                .Select(method => new[] { method.Name, method, benchmarkInstance });
        }
    }
}
