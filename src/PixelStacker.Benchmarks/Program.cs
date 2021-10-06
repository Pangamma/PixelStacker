using BenchmarkDotNet.Running;
using BenchmarkDotNet2Highcharts;
using PixelStacker.Benchmarks.ColorMap;

namespace PixelStacker.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // var b =           new BestMatch();
            //b.AverageColorBruteForceMapper_BestMatch();
            var sumary = BenchmarkRunner.Run<BestMatch>();
            var derp2 = BenchmarkRunner.Run<BestMatches>();
            //BenchmarkRunner.Run<Dummy>();
            //BenchmarkRunner.Run<Dummy2>();
            var hc = new HighchartsExporter();
            hc.Export();
        }
    }
}
