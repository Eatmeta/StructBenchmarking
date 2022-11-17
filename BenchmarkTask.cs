using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
    {
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            var timer = new Stopwatch();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            task.Run();
            timer.Start();
            for (var i = 0; i < repetitionCount; i++)
                task.Run();
            timer.Stop();
            return timer.Elapsed.TotalMilliseconds / repetitionCount;
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        private class BuilderTest : ITask
        {
            public void Run()
            {
                var builder = new StringBuilder();
                for (var i = 0; i < 10000; i++)
                    builder.Append("a");
                var temp = builder.ToString();
                builder.Clear();
            }
        }

        private class StringTest : ITask
        {
            public void Run()
            {
                var temp = new string('a', 10000);
            }
        }

        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var builderTest = new BuilderTest();
            var stringTest = new StringTest();
            var benchmark = new Benchmark();
            var builderTestResult = benchmark.MeasureDurationInMs(builderTest, 10);
            var stringTestResult = benchmark.MeasureDurationInMs(stringTest, 10);
            Assert.Less(stringTestResult, builderTestResult);
        }
    }
}