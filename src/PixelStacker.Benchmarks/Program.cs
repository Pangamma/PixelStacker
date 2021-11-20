using BenchmarkDotNet.Running;
using PixelStacker.Benchmarks.ColorMap;

namespace PixelStacker.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // var b =           new BestMatch();
            //b.AverageColorBruteForceMapper_BestMatch();
            //var sumary = BenchmarkRunner.Run<KdTreeBenchmarks>();
            BenchmarkRunner.Run<BestMatch>();
            //BenchmarkRunner.Run<BestMatches>();
            //BenchmarkRunner.Run<Dummy>();
            //BenchmarkRunner.Run<Dummy2>();
        }
    }
}
