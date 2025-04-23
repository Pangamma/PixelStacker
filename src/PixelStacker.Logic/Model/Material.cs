using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PixelStacker.Logic.Model
{
    [Serializable]
    public class Material
    {
        public string PixelStackerID { get; set; }
        public string Label { get; set; }
        public SKBitmap SideImage { get; private set; }
        public SKBitmap TopImage { get; private set; }
        public string Category { get; set; }
        public string SchematicaMaterialName { get; set; }
        public bool IsAdvanced { get; set; } = false;

        /// <summary>
        /// True only if AIR.
        /// </summary>
        public bool IsAir { get; set; }

        /// <summary>
        /// True if glass or other block that goes on the top layer.
        /// These blocks are always transparent or see through.
        /// </summary>
        public bool CanBeUsedAsTopLayer { get; }

        /// <summary>
        /// True if dirt or other block that goes on the bottom layer.
        /// These blocks are always solid and not transparent.
        /// </summary>
        public bool CanBeUsedAsBottomLayer { get; }


        /// <summary>
        /// minecraft:stone_1
        /// </summary>
        private string TopBlockName { get; set; }

        /// <summary>
        /// minecraft:stone_1[axis=y]
        /// </summary>
        private string SideBlockName { get; set; }

        private SKColor? _averageColor = null;
        private SKColor? _averageColorSide = null;

        private double? _roughness = null;
        private double? _roughnessSide = null;
        public double GetRoughness(bool isSide)
        {
            if (isSide)
            {
                if (_roughnessSide == null)
                {
                    var avg = this.GetAverageColor(isSide);
                    var colorFreqs = this.GetImage(isSide).GetColorsInImage()
                        .GroupBy(x => x)
                        .Select(x => new Tuple<SKColor, int>(x.Key, x.Count()))
                        .ToList();

                    _roughnessSide = avg.GetAverageColorDistance(colorFreqs);
                }

                return _roughnessSide.Value;
            }
            else
            {
                if (_roughness == null)
                {
                    var avg = this.GetAverageColor(isSide);
                    var colorFreqs = this.GetImage(isSide).GetColorsInImage()
                        .GroupBy(x => x)
                        .Select(x => new Tuple<SKColor, int>(x.Key, x.Count()))
                        .ToList();

                    _roughness = avg.GetAverageColorDistance(colorFreqs);
                }
                return _roughness.Value;
            }
        }

        /// <summary>
        /// List of words or phrases ppl can search for to get these materials
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        private static readonly string[] ValidMinecraftVersions = new string[] {
            "NEW", "1.7", // 1.7 and 1.12 will contain inaccuracies where I was originally "rounding" up or down.
            "1.8", "1.9", "1.10",
            "1.12", "1.13", "1.14", "1.15", "1.16",
            "1.17", "1.19", "1.20", "1.21.5"
        };

        private string _minimumSupportedMinecraftVersion = "NEW";
        public string MinimumSupportedMinecraftVersion
        {
            get => _minimumSupportedMinecraftVersion;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !ValidMinecraftVersions.Contains(value))
                {
                    throw new ArgumentNullException($"Invalid MC Version provided. Given '{value}' Expected: {string.Join(", ", ValidMinecraftVersions)}");
                }

                _minimumSupportedMinecraftVersion = value;
            }
        }

        internal Material(string minMcVersion, bool isAdvancedMaterial, string category, string pixelStackerID, string label, byte[] topImage, byte[] sideImage, string topBlockName, string sideBlockName, string schematicaMaterialName)
           : this(minMcVersion, isAdvancedMaterial, category, pixelStackerID, label, SKBitmap.Decode(topImage), SKBitmap.Decode(sideImage), topBlockName, sideBlockName, schematicaMaterialName)
        { }

        internal Material(string minMcVersion, bool isAdvancedMaterial, string category, string pixelStackerID, string label, SKBitmap topImage, SKBitmap sideImage, string topBlockName, string sideBlockName, string schematicaMaterialName)
        {
            MinimumSupportedMinecraftVersion = minMcVersion;
            IsAdvanced = isAdvancedMaterial;
            PixelStackerID = pixelStackerID;
            Label = label;
            TopImage = topImage;
            SideImage = sideImage ?? topImage;
            Category = category;
            TopBlockName = topBlockName;
            SideBlockName = sideBlockName;
            SchematicaMaterialName = schematicaMaterialName;
            IsAir = pixelStackerID == "AIR";
            CanBeUsedAsTopLayer = category == "Glass" || category == "Transparent";
            CanBeUsedAsBottomLayer = !IsAir && !CanBeUsedAsTopLayer;
        }

        private string SettingsKey { get { return string.Format("BLOCK_{0}", PixelStackerID); } }

        public bool IsVisibleF(Options opts)
        {
            if (PixelStackerID == "AIR")
            {
                return false;
            }

            if (!opts.IsAdvancedModeEnabled && IsAdvanced)
            {
                return false;
            }

            return true;
        }

        public bool IsEnabledF(Options opts)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            opts ??= Options.GetInMemoryFallback;
#pragma warning restore CS0618 // Type or member is obsolete
            if (PixelStackerID == "AIR")
            {
                return false;
            }

            if (IsAdvanced && !opts.IsAdvancedModeEnabled)
            {
                return false;
            }

            if (!opts.EnableStates.ContainsKey(SettingsKey))
            {
                opts.EnableStates[SettingsKey] = !IsAdvanced;
            }

            return opts.EnableStates[SettingsKey];
        }

        public void IsEnabledF(Options opts, bool val)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            opts ??= Options.GetInMemoryFallback;
#pragma warning restore CS0618 // Type or member is obsolete
            opts.EnableStates[SettingsKey] = val;
        }

        public string GetBlockNameAndData(bool isSide)
        {
            return isSide ? SideBlockName : TopBlockName;
        }

        public SKBitmap GetImage(bool isSide)
        {
            if (isSide)
            {
                return SideImage;
            }
            else
            {
                return TopImage;
            }
        }

        public SKColor GetAverageColor(bool isSide)
        {
            if (isSide)
            {
                if (_averageColorSide == null)
                {
                    _averageColorSide = GetAverageColor(SideImage);
                }
                return _averageColorSide.Value;
            }
            else
            {
                if (_averageColor == null)
                {
                    _averageColor = GetAverageColor(TopImage);
                }
                return _averageColor.Value;
            }
        }

        private SKColor GetAverageColor(SKBitmap src, int rgbFragmentSize = 1)
        {
            long r = 0;
            long g = 0;
            long b = 0;
            long a = 0;
            long total = src.Width * src.Height;

            src.ToViewStream(null, (x, y, c) =>
            {
                Interlocked.Add(ref r, c.Red);
                Interlocked.Add(ref g, c.Green);
                Interlocked.Add(ref b, c.Blue);
                Interlocked.Add(ref a, c.Alpha);
            });

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            if (a > 128)
            {
                a = 255;
            }

            return new SKColor((byte)r, (byte)g, (byte)b, (byte)a).Normalize(rgbFragmentSize);
        }

        public override string ToString()
        {
            return PixelStackerID;
        }

        public override bool Equals(object obj)
        {
            var material = obj as Material;
            return material != null &&
                   PixelStackerID == material.PixelStackerID;
        }

        public override int GetHashCode()
        {
            return 981597221 + EqualityComparer<string>.Default.GetHashCode(PixelStackerID);
        }

        public static bool operator ==(Material left, Material right)
        {
            return left?.PixelStackerID == right?.PixelStackerID;
        }

        public static bool operator !=(Material left, Material right)
        {
            return left?.PixelStackerID != right?.PixelStackerID;
        }
    }
}
