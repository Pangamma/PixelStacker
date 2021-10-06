using BenchmarkDotNet.Attributes;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using PixelStacker.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    [SimpleJob(launchCount: 1, warmupCount: 2, targetCount: 2)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn, IterationsColumn]
    public class BaseColorMapBenchmarks
    {
        public BaseColorMapBenchmarks()
        {
            this.MaterialPalette = ResxHelper.LoadJson<MaterialPalette>(DataResources.materialPalette);
            this.EnabledMaterials = MaterialPalette.ToCombinationList();
        }

        public MaterialPalette MaterialPalette { get; }
        public List<MaterialCombination> EnabledMaterials { get; }
    }
}
