using PixelStacker.Logic;
using PixelStacker.UI;
using SimplePaletteQuantizer.ColorCaches;
using SimplePaletteQuantizer.ColorCaches.Common;
using SimplePaletteQuantizer.ColorCaches.EuclideanDistance;
using SimplePaletteQuantizer.ColorCaches.LocalitySensitiveHash;
using SimplePaletteQuantizer.ColorCaches.Octree;
using SimplePaletteQuantizer.Ditherers;
using SimplePaletteQuantizer.Ditherers.ErrorDiffusion;
using SimplePaletteQuantizer.Ditherers.Ordered;
using SimplePaletteQuantizer.Helpers;
using SimplePaletteQuantizer.Quantizers;
using SimplePaletteQuantizer.Quantizers.DistinctSelection;
using SimplePaletteQuantizer.Quantizers.MedianCut;
using SimplePaletteQuantizer.Quantizers.NeuQuant;
using SimplePaletteQuantizer.Quantizers.Octree;
using SimplePaletteQuantizer.Quantizers.OptimalPalette;
using SimplePaletteQuantizer.Quantizers.Popularity;
using SimplePaletteQuantizer.Quantizers.Uniform;
using SimplePaletteQuantizer.Quantizers.XiaolinWu;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePaletteQuantizer
{
    public class QuantizerEngine
    {
        private ConcurrentDictionary<Color, long> errorCache;
        private OrderedDictionary<string, IColorQuantizer> algorithmList;
        private OrderedDictionary<string, IColorDitherer> dithererList;
        private OrderedDictionary<string, IColorCache> colorCacheList;
        private OrderedDictionary<string, ColorModel> colorModelList;
        public static QuantizerEngine _self = null;
        public static QuantizerEngine Get { get { if (_self == null) { _self = new QuantizerEngine(); } return _self; } }

        public QuantizerEngine()
        {
            errorCache = new ConcurrentDictionary<Color, Int64>();

            algorithmList = new OrderedDictionary<string, IColorQuantizer>
            {
                {"HSL distinct selection", new DistinctSelectionQuantizer() },
                {"Uniform quantization", new UniformQuantizer() },
                {"Popularity algorithm", new PopularityQuantizer() },
                {"Median cut algorithm", new MedianCutQuantizer() },
                {"Octree quantization", new OctreeQuantizer() },
                {"Wu's color quantizer", new WuColorQuantizer() },
                { "Neural quantizer", new NeuralColorQuantizer() },
                { "NeuQuant quantizer" , new NeuralColorQuantizer()},
                { "Optimal palette", new OptimalPaletteQuantizer() }
            };

            dithererList = new OrderedDictionary<string, IColorDitherer>
            {
                { "No dithering", null },
                { "--[ Ordered ]--", null },
                { "Bayer dithering (4x4)", new BayerDitherer4() },
                { "Bayer dithering (8x8)", new BayerDitherer8() },
                { "Clustered dot (4x4)", new ClusteredDotDitherer() },
                { "Dot halftoning (8x8)", new DotHalfToneDitherer() },
                { "--[ Error diffusion ]--", null },
                { "Fan dithering (7x3)", new FanDitherer() },
                { "Shiau dithering (5x3)", new ShiauDitherer() },
                { "Sierra dithering (5x3)", new SierraDitherer() },
                { "Stucki dithering (5x5)", new StuckiDitherer() },
                { "Burkes dithering (5x3)", new BurkesDitherer() },
                { "Atkinson dithering (5x5)", new AtkinsonDithering() },
                { "Two-row Sierra dithering (5x3)", new TwoRowSierraDitherer() },
                { "Floyd–Steinberg dithering (3x3)", new FloydSteinbergDitherer() },
                { "Jarvis-Judice-Ninke dithering (5x5)", new JarvisJudiceNinkeDitherer() },
            };

            colorCacheList = new OrderedDictionary<string, IColorCache>
            {
                { "Euclidean distance", new EuclideanDistanceColorCache() },
                { "Locality-sensitive hash", new LshColorCache () },
                {"Octree search", new OctreeColorCache() }
            };

            colorModelList = new OrderedDictionary<string, ColorModel>
            {
                {"RGB", ColorModel.RedGreenBlue },
                {"LAB", ColorModel.LabColorSpace },
            };

            var activeQuantizer = GetActiveQuantizer();
            var activeColorCache = GetActiveColorCache();
            foreach (var q in algorithmList.Values)
            {
                if (q is BaseColorCacheQuantizer)
                {
                    ((BaseColorCacheQuantizer)q).ChangeCacheProvider(activeColorCache);
                }
            }

        }

        public void EnforceValidSettings(PreRenderOptionsForm form)
        {
            var aQ = GetActiveQuantizer();
            var aCsh = GetActiveColorCache();

            // First load (ish)
            form.ddlAlgorithm.Enabled = true;
            form.ddlColorCache.Enabled = aQ is BaseColorCacheQuantizer;
            form.ddlDither.Enabled = !(aQ is WuColorQuantizer);

            form.ddlParallel.Enabled = aQ.AllowParallel;
            if (aQ.AllowParallel == false) form.ddlParallel.SelectedItem = "1";
            if (aQ.AllowParallel == false) Options.Get.PreRender_Parallel = 1;

            // If Quantizer changes
            this.algorithmList.Values.OfType<BaseColorCacheQuantizer>().ToList().ForEach(x => x.ChangeCacheProvider(aCsh));

            // Color Count
            Boolean allowColors = !(aQ is UniformQuantizer || aQ is NeuralColorQuantizer || aQ is OptimalPaletteQuantizer);
            form.ddlColorCount.Enabled = allowColors;
            if (allowColors == false)
            {
                // Also disable the DDL, but that is already done.
                form.ddlColorCount.SelectedItem = "256";
                Options.Get.PreRender_ColorCount = 256;
            }
        }

        public IColorQuantizer GetActiveQuantizer(string chosen = null)
        {
            chosen = chosen ?? Options.Get.PreRender_Algorithm;
            IColorQuantizer result = this.algorithmList.ContainsKey(chosen) ? this.algorithmList[chosen] : this.algorithmList.Values.FirstOrDefault();
            return result;
        }

        public IColorDitherer GetActiveDitherer(string chosen = null)
        {
            chosen = chosen ?? Options.Get.PreRender_Dither;
            IColorDitherer result = this.dithererList.ContainsKey(chosen) ? this.dithererList[chosen] : this.dithererList.Values.FirstOrDefault();
            return result;
        }

        public IColorCache GetActiveColorCache(string chosen = null)
        {
            chosen = chosen ?? Options.Get.PreRender_ColorCache;
            IColorCache result = this.colorCacheList.ContainsKey(chosen) ? this.colorCacheList[chosen] : this.colorCacheList.Values.FirstOrDefault();
            return result;
        }

        /// <summary>
        ///  SOURCE IMAGE SHOULD BE 32bbpARGB
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <returns></returns>
        public async Task<Bitmap> RenderImage(Bitmap sourceImage)
        {
            //// prepares quantizer
            errorCache.Clear();

            // tries to retrieve an image based on HSB quantization
            var activeQuantizer = this.GetActiveQuantizer();
            var activeDitherer = this.GetActiveDitherer();
            Int32 parallelTaskCount = activeQuantizer.AllowParallel ? Options.Get.PreRender_Parallel : 1;
            Int32 colorCount = Options.Get.PreRender_ColorCount;

            //TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            
            // For some reason the super quick algo failed. Need to fail over to this super safe one.
            using (Image targetImage = ImageBuffer.QuantizeImage(sourceImage, activeQuantizer, activeDitherer, colorCount, parallelTaskCount))
            {
                Bitmap output = targetImage.To32bppBitmap();
                ApplyBitmaskForTransparency(sourceImage, output);
                return output;
            }

        }

        private static void ApplyBitmaskForTransparency_Original(Bitmap origImage, Bitmap dstImage)
        {
            //Get the bitmap data
            var bitmapData = origImage.LockBits(
                new Rectangle(0, 0, origImage.Width, origImage.Height),
                ImageLockMode.ReadWrite,
                origImage.PixelFormat
            );

            //Initialize an array for all the image data
            byte[] imageBytes = new byte[bitmapData.Stride * origImage.Height];

            //Copy the bitmap data to the local array
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, imageBytes, 0, imageBytes.Length);

            //Unlock the bitmap
            origImage.UnlockBits(bitmapData);

            //Find pixelsize
            int pixelSize = Image.GetPixelFormatSize(origImage.PixelFormat);

            // An example on how to use the pixels, lets make a copy
            int x = 0;
            int y = 0;
            //Loop pixels
            for (int i = 0; i < imageBytes.Length; i += pixelSize / 8)
            {
                //Copy the bits into a local array
                var pixelData = new byte[4];
                Array.Copy(imageBytes, i, pixelData, 0, 4);

                //Get the color of a pixel
                var color = Color.FromArgb(pixelData[3], pixelData[0], pixelData[1], pixelData[2]);

                if (color.A < 32)
                {
                    dstImage.SetPixel(x, y, Color.Transparent);
                }
                
                //Map the 1D array to (x,y)
                x++;
                if (x >= origImage.Width)
                {
                    x = 0;
                    y++;
                }

            }
        }

        private static void ApplyBitmaskForTransparency(Bitmap origImage, Bitmap dstImage)
        {
            try
            {
                origImage.ToMergeStream(dstImage, null, (int x, int y, Color o, Color n) => {
                    if (o.A < 32) return Color.Transparent;
                    else return n;
                });
            }
            catch (ArgumentOutOfRangeException)
            {
                for (int x = 0; x < origImage.Width; x++)
                {
                    for (int y = 0; y < origImage.Height; y++)
                    {
                        Color c = origImage.GetPixel(x, y);
                        if (c.A < 32)
                        {
                            dstImage.SetPixel(x, y, Color.Transparent);
                        }
                    }
                }
            }
        }

        private static Image GetConvertedImage(Image image, ImageFormat newFormat, out Int32 imageSize)
        {
            Image result;

            // saves the image to the stream, and then reloads it as a new image format; thus conversion.. kind of
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, newFormat);
                stream.Seek(0, SeekOrigin.Begin);
                imageSize = (Int32)stream.Length;
                result = Image.FromStream(stream);
            }

            return result;
        }
    }
}
