using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility.NodeParse;

namespace BenchmarksTest.Benchmarks
{
    [MemoryDiagnoser]
    public class TagValidBenchmark
    {
        private DefineContent defineContent;

        [Params("E[6][1:5]", "E[6][1:8]","D.d", "D.c[2:5]", "D.b[8].d[1][2:5].c[2][1:3]")]
        public string Path { get; set; }

        [Benchmark]
        public void ValidPath()
        {
            var segments = NodeValid.Parse(Path);
            NodeValid.IsPathValid(segments, defineContent.TagMap, defineContent.StructMap);
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            defineContent = NodeValid.BuildStructMap();
        }

        /*
        | Method    | Path                 | Mean     | Error    | StdDev   | Gen0   | Allocated |
        |---------- |--------------------- |---------:|---------:|---------:|-------:|----------:|
        | ValidPath | D.b[8(...)[1:3] [26] | 895.9 ns | 13.40 ns | 12.53 ns | 0.4025 |    2536 B |
        | ValidPath | D.c[2:5]             | 286.4 ns |  5.67 ns |  6.06 ns | 0.1326 |     832 B |
        | ValidPath | D.d                  | 131.1 ns |  2.59 ns |  2.98 ns | 0.0763 |     480 B |
        | ValidPath | E[6][1:5]            | 367.1 ns |  5.85 ns |  5.47 ns | 0.1593 |    1000 B |
        | ValidPath | E[6][1:8]            | 368.0 ns |  7.20 ns |  7.07 ns | 0.1593 |    1000 B |
         */
    }
}
