using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Factory
{
    public class PixelStackerFactory
    {
        internal IColorMapper Mapper;
        internal MaterialPalette Palette;
        internal CanvasPreprocessorSettings Preprocess;
        internal bool IsSideView;

        public static PixelStackerFactoryBuilder Builder() => new PixelStackerFactoryBuilder();

        internal PixelStackerFactory() { }
        public async Task<byte[]> ExportAsync(System.Threading.CancellationToken? worker, ExportFormat format, SKBitmap img)
        {
            IExportFormatter exporter = format.GetFormatter();
            var engine = new RenderCanvasEngine();
            var imgPreprocessed = await engine.PreprocessImageAsync(worker, img, Preprocess);
            var canvas = await engine.RenderCanvasAsync(worker, imgPreprocessed, this.Mapper, this.Palette, this.IsSideView);
            byte[] data = await exporter.ExportAsync(new PixelStackerProjectData()
            {
                CanvasData = canvas.CanvasData,
                IsSideView = this.IsSideView,
                MaterialPalette = Palette,
                PreprocessedImage = imgPreprocessed,
                WorldEditOrigin = canvas.WorldEditOrigin
            }, worker);
            return data;
        }
    }

    public class PxBuilderPreprocessed
    {
        internal IColorMapper Mapper;
        internal MaterialPalette Palette;
        internal CanvasPreprocessorSettings Preprocess;
        internal bool IsSideView;

        internal PxBuilderPreprocessed() { }
        public PixelStackerFactory Build() => new PixelStackerFactory()
        {
            Palette = this.Palette,
            Mapper = this.Mapper,
            Preprocess = this.Preprocess,
            IsSideView = this.IsSideView
        };
    }

    public class PxBuilderSeededColorMapper
    {
        internal IColorMapper Mapper;
        internal MaterialPalette Palette;
        internal bool IsSideView;
        private CanvasPreprocessorSettings Preproc = new CanvasPreprocessorSettings()
        {
            RgbBucketSize = 1,
            QuantizerSettings = new QuantizerSettings()
            {
                IsEnabled = false
            }
        };

        internal PxBuilderSeededColorMapper() { }

        public PxBuilderPreprocessed SkipPreprocessing()
        {
            return new PxBuilderPreprocessed()
            {
                Palette = this.Palette,
                Mapper = this.Mapper,
                Preprocess = this.Preproc
            };
        }

        public PxBuilderSeededColorMapper RgbBucketSize(int bucketSize)
        {
            switch (bucketSize)
            {
                case 1:
                case 5:
                case 15:
                case 17:
                case 51:
                    this.Preproc.RgbBucketSize = bucketSize;
                    return this;
                default: throw new ArgumentOutOfRangeException(nameof(bucketSize), "Valid values are [1, 5, 15, 17, 51].");
            }
        }

        public PxBuilderSeededColorMapper MaxDimensions(int? maxWidth, int? maxHeight)
        {
            this.Preproc.MaxWidth = maxWidth;
            this.Preproc.MaxHeight = maxHeight;
            return this;
        }

        public PxBuilderPreprocessed Build()
        {
            return new PxBuilderPreprocessed()
            {
                Preprocess = this.Preproc,
                Mapper = this.Mapper,
                IsSideView = this.IsSideView,
                Palette = this.Palette,
            };
        }


        //public PixelBuilderPreprocessed WithPreprocessing(CanvasPreprocessorSettings settings)
        //{

        //}

        //    Options opts = new MemoryOptionsProvider().Load();

        //        var engine = new RenderCanvasEngine();

        //        this.Canvases["Fast"] = new AsyncLazy<RenderedCanvas>(async () => {
        //            var img = await engine.PreprocessImageAsync(null,
        //                DevResources.pink_girl,
        //                new CanvasPreprocessorSettings()
        //                {
        //                    RgbBucketSize = 15,
        //                    MaxHeight = 10,
        //                    QuantizerSettings = new QuantizerSettings()
        //                    {
        //                        Algorithm = QuantizerAlgorithm.WuColor,
        //                        MaxColorCount = 64,
        //                        IsEnabled = false,
        //                        DitherAlgorithm = "No dithering"
        //                    }
        //                });


        //            return await engine.RenderCanvasAsync(null, img, mapper, palette);
        //});
    }

    public class PxBuilderColorMapper
    {
        internal IColorMapper Mapper;
        internal MaterialPalette Palette;
        internal bool IsSideView;

        internal PxBuilderColorMapper() { }

        public PxBuilderSeededColorMapper SetSeedData(List<MaterialCombination> enabledCombinations)
        {
            Mapper.SetSeedData(enabledCombinations, Palette, this.IsSideView);
            return new PxBuilderSeededColorMapper()
            {
                Mapper = this.Mapper,
                Palette = this.Palette,
                IsSideView = this.IsSideView
            };
        }

        public PxBuilderSeededColorMapper SetSeedData(Options opts)
        {
            Mapper.SetSeedData(Palette.ToValidCombinationList(opts), Palette, opts.IsSideView);
            return new PxBuilderSeededColorMapper()
            {
                Mapper = this.Mapper,
                Palette = this.Palette,
                IsSideView = this.IsSideView
            };
        }

        public PxBuilderSeededColorMapper SetSeedData()
        {
            var opts = new StaticJsonOptionsProvider().Load();
            Mapper.SetSeedData(Palette.ToValidCombinationList(opts), Palette, this.IsSideView);
            return new PxBuilderSeededColorMapper()
            {
                Mapper = this.Mapper,
                Palette = this.Palette,
                IsSideView = this.IsSideView
            };
        }

        public PxBuilderSeededColorMapper SkipSeedData()
        {
            if (!this.Mapper.IsSeeded())
            {
                throw new Exception("Mapper must be seeded manually outside of this method.");
            }

            return new PxBuilderSeededColorMapper()
            {
                Mapper = this.Mapper,
                Palette = this.Palette,
                IsSideView = this.IsSideView
            };
        }
    }

    public class PxBuilderOriented
    {
        private bool IsSideView = false;
        internal PxBuilderOriented() { }

        public PxBuilderColorMapper WithColorMapper(IColorMapper mapper)
        {
            return new PxBuilderColorMapper()
            {
                Mapper = mapper,
                Palette = MaterialPalette.FromResx(),
                IsSideView = this.IsSideView,
            };
        }

        public PxBuilderColorMapper WithColorMapper<T>() where T : IColorMapper, new()
        => WithColorMapper(new T());
    }

    public class PixelStackerFactoryBuilder
    {
        internal PixelStackerFactoryBuilder() { }
        public PxBuilderOriented WithOrientation(bool isSideView) => new PxBuilderOriented();

        public PxBuilderColorMapper WithColorMapper<T>() where T : IColorMapper, new() => WithColorMapper(new T());
        public PxBuilderColorMapper WithColorMapper(IColorMapper mapper) => new PxBuilderColorMapper()
        {
            Mapper = mapper,
            Palette = MaterialPalette.FromResx(),
            IsSideView = false
        };

    }
}
