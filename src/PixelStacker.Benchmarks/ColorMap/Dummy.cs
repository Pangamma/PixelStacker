using BenchmarkDotNet.Attributes;
using System.Threading.Tasks;

namespace PixelStacker.Benchmarks.ColorMap
{

    [HtmlExporter, CsvMeasurementsExporter, RPlotExporter()]
    //[ShortRunJob]
    [SimpleJob(launchCount: 1, warmupCount: 1)]
    [BaselineColumn, MinColumn, MaxColumn,
        //MeanColumn,
        MedianColumn, IterationsColumn]
    public class Dummy2
    {
        [Benchmark(Baseline = true)]
        public void AverageColorBruteForceMapper_BestMatch()
        {
            Task.Delay(100);
        }

        [Benchmark]
        public void SeparateColorBruteForceMapper_BestMatch()
        {
            Task.Delay(200);
        }
    }

    [HtmlExporter, CsvMeasurementsExporter, RPlotExporter()]
    //[ShortRunJob]
    [SimpleJob(launchCount: 1, warmupCount: 1)]
    [BaselineColumn, MinColumn, MaxColumn, 
        //MeanColumn,
        MedianColumn, IterationsColumn]
    public class Dummy
    {
        [Benchmark(Baseline = true)]
        public void AverageColorBruteForceMapper_BestMatch()
        {
            Task.Delay(100);
        }

        [Benchmark]
        public void SeparateColorBruteForceMapper_BestMatch()
        {
            Task.Delay(200);
        }
    }
}
