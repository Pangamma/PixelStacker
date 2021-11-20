using BenchmarkDotNet.Attributes;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using System.Collections.Generic;

namespace PixelStacker.Benchmarks.ColorMap
{
    [AsciiDocExporter]
    //[CsvExporter]
    //[CsvMeasurementsExporter]
    [HtmlExporter]
    [PlainExporter]
    //[RPlotExporter]
    //[MarkdownExporter]
    //[ShortRunJob]

    [JsonExporterAttribute.Brief]
    //[SimpleJob(launchCount: 1, warmupCount: 2, targetCount: 2)]
    [Config(typeof(FastAndDirtyConfig))]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn, IterationsColumn]
    public class BaseColorMapBenchmarks
    {
        public BaseColorMapBenchmarks()
        {
            this.MaterialPalette = ResxHelper.LoadJson<MaterialPalette>(Resources.Data.materialPalette);
            this.EnabledMaterials = MaterialPalette.ToCombinationList();
        }

        public MaterialPalette MaterialPalette { get; }
        public List<MaterialCombination> EnabledMaterials { get; }
    }
}
