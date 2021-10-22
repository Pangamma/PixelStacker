using PixelStacker.Core.IO.Config;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Model.Mats
{
    [Serializable]
    public class Material
    {
        public string PixelStackerID { get; set; }
        public string Label { get; set; }
        public int BlockID { get; set; }
        public int Data { get; set; }
        public PxBitmap SideImage { get; private set; }
        public PxBitmap TopImage { get; private set; }
        public string Category { get; set; }
        public string SchematicaMaterialName { get; set; }
        public bool IsAdvanced { get; set; } = false;

        /// <summary>
        /// minecraft:stone_1
        /// </summary>
        private string TopBlockName { get; set; }

        /// <summary>
        /// minecraft:stone_1[axis=y]
        /// </summary>
        private string SideBlockName { get; set; }

        private PxColor? _averageColor = null;
        private PxColor? _averageColorSide = null;

        /// <summary>
        /// List of words or phrases ppl can search for to get these materials
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        private static readonly string[] ValidMinecraftVersions = new string[] {
            "NEW", "1.7", // 1.7 and 1.12 will contain inaccuracies where I was originally "rounding" up or down.
            "1.8", "1.9", "1.10",
            "1.12", "1.13", "1.14", "1.15", "1.16",
            "1.17"
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

        public Material(string minMcVersion, bool isAdvancedMaterial, string category, string pixelStackerID, string label, int blockID, int data, PxBitmap topImage, PxBitmap sideImage, string topBlockName, string sideBlockName, string schematicaMaterialName)
        {
            MinimumSupportedMinecraftVersion = minMcVersion;
            IsAdvanced = isAdvancedMaterial;
            PixelStackerID = pixelStackerID;
            Label = label;
            BlockID = blockID;
            Data = data;
            TopImage = topImage; // .To32bppBitmap();
            SideImage = sideImage ?? topImage; // .To32bppBitmap();
            Category = category;
            TopBlockName = topBlockName;
            SideBlockName = sideBlockName;
            SchematicaMaterialName = schematicaMaterialName;
        }

        private string SettingsKey => PixelStackerID;


        public bool IsVisibleF(Options opts)
        {
            if (BlockID == 0)
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
            //opts ??= Options.Get;
#pragma warning restore CS0618 // Type or member is obsolete
            if (BlockID == 0)
            {
                return false;
            }

            if (IsAdvanced && !opts.IsAdvancedModeEnabled)
            {
                return false;
            }

            bool val;
            if (!opts.Materials.MaterialIDs.TryGetValue(SettingsKey, out val))
            {
                opts.Materials.MaterialIDs[SettingsKey] = !IsAdvanced && BlockID != 0;
                val = !IsAdvanced && BlockID != 0;
            }

            return val;
        }

        public void IsEnabledF(Options opts, bool val)
        {
            opts.Materials.MaterialIDs[SettingsKey] = val;
        }

        public string GetBlockNameAndData(bool isSide)
        {
            return isSide ? SideBlockName : TopBlockName;
        }

        public PxBitmap GetImage(bool isSide)
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

        public PxColor GetAverageColor(bool isSide)
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

        private PxColor GetAverageColor(PxBitmap src, int rgbFragmentSize = 1)
        {
            long r = 0;
            long g = 0;
            long b = 0;
            long a = 0;
            long total = src.Width * src.Height;

            foreach (var kvp in src)
            {
                r += kvp.Data.R;
                g += kvp.Data.G;
                b += kvp.Data.B;
                a += kvp.Data.A;
            }

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            if (a > 128)
            {
                a = 255;
            }

            return PxColor.FromArgb((int)a, (int)r, (int)g, (int)b).Normalize(rgbFragmentSize);
        }

        public override string ToString()
        {
            return Label;
        }

        public override bool Equals(object obj)
        {
            var material = obj as Material;
            return material != null &&
                   Label == material.Label;
        }

        public override int GetHashCode()
        {
            return 981597221 + EqualityComparer<string>.Default.GetHashCode(Label);
        }

        public static bool operator ==(Material left, Material right)
        {
            return left?.ToString() == right?.ToString();
        }

        public static bool operator !=(Material left, Material right)
        {
            return left?.ToString() != right?.ToString();
        }
    }
}
