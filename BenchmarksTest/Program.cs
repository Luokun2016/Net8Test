using BenchmarkDotNet.Running;
using BenchmarksTest.Benchmarks;

namespace BenchmarksTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<JsonBenchmarks>();
            var summary2 = BenchmarkRunner.Run<TagValidBenchmark>();
        }
    }
}
