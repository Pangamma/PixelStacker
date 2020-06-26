using ColorMine.ColorSpaces;
using PixelStacker.Logic.Extensions;
using PixelStacker.Properties;
using PixelStacker.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    [Serializable]
    public class Material
    {
        public string PixelStackerID { get; set; }
        public string Label { get; set; }
        public int BlockID { get; set; }
        public int Data { get; set; }
        public Bitmap SideImage { get; private set; }
        public Bitmap TopImage { get; private set; }
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

        private Color? _averageColor = null;
        private Color? _averageColorSide = null;

        /// <summary>
        /// List of words or phrases ppl can search for to get these materials
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        private static readonly string[] ValidMinecraftVersions = new string[] {
            "NEW", "1.7", // 1.7 and 1.12 will contain inaccuracies where I was originally "rounding" up or down.
            "1.8", "1.9", "1.10",
            "1.12", "1.13", "1.14", "1.15", "1.16"
        };
        private string _minimumSupportedMinecraftVersion = "NEW";
        public string MinimumSupportedMinecraftVersion
        {
            get => this._minimumSupportedMinecraftVersion;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !ValidMinecraftVersions.Contains(value))
                {
                    throw new ArgumentNullException($"Invalid MC Version provided. Given '{value}' Expected: {string.Join(", ", Material.ValidMinecraftVersions)}");
                }

                _minimumSupportedMinecraftVersion = value;
            }
        }

        public Material(string minMcVersion, bool isAdvancedMaterial, string category, string pixelStackerID, string label, int blockID, int data, Bitmap topImage, Bitmap sideImage, string topBlockName, string sideBlockName, string schematicaMaterialName)
        {
            this.MinimumSupportedMinecraftVersion = minMcVersion;
            this.IsAdvanced = isAdvancedMaterial;
            this.PixelStackerID = pixelStackerID;
            this.Label = label;
            this.BlockID = blockID;
            this.Data = data;
            this.TopImage = topImage;
            this.SideImage = sideImage ?? topImage;
            this.Category = category;
            this.TopBlockName = topBlockName;
            this.SideBlockName = sideBlockName;
            this.SchematicaMaterialName = schematicaMaterialName;
        }

        public string toConstructorString()
        {
            ResourceSet resources = Textures.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, false, true);
            DictionaryEntry? topImageResource = null;
            DictionaryEntry? sideImageResource = null;

            foreach (DictionaryEntry resource in resources)
            {
                if (resource.Value is Bitmap)
                {
                    if (topImageResource == null)
                    {
                        if ((resource.Value as Bitmap).AreEqual(this.TopImage))
                        {
                            topImageResource = resource;
                        }
                    }

                    if (sideImageResource == null)
                    {
                        if ((resource.Value as Bitmap).AreEqual(this.SideImage))
                        {
                            sideImageResource = resource;
                        }
                    }
                }
            }

            if (topImageResource == null)
            {
                throw new Exception("Top image not found in list");
            }

            if (sideImageResource == null)
            {
                throw new Exception("Side image not found in list");
            }

            string topBlockName = this.TopBlockName.Replace("minecraft:" + topImageResource.Value.Key.ToString(), "minecraft:{nameof(Textures." + topImageResource.Value.Key.ToString() + ")}");
            string sideBlockName = this.SideBlockName.Replace("minecraft:" + topImageResource.Value.Key.ToString(), "minecraft:{nameof(Textures." + topImageResource.Value.Key.ToString() + ")}");
       
            return $"new Material("
                + $"\"{this.MinimumSupportedMinecraftVersion}\", "
                + $"{this.IsAdvanced.ToString().ToLowerInvariant()}, "
                + $"\"{this.Category}\", "
                + $"\"{this.PixelStackerID}\", "
                + $"\"{this.Label}\", "
                + $"{this.BlockID}, "
                + $"{this.Data}, "
                + $"Textures.{topImageResource.Value.Key}, "
                + $"Textures.{sideImageResource.Value.Key}, "
                + $"$\"{topBlockName}\", "
                + $"$\"{sideBlockName}\", "
                + $"\"{this.SchematicaMaterialName}\""
                + "),";
        }

        private string SettingsKey { get { return string.Format("BLOCK_{0}", this.PixelStackerID); } }

        public bool IsVisible
        {
            get
            {
                if (this.IsAdvanced)
                {

                }
                if (!Options.Get.IsAdvancedModeEnabled && this.IsAdvanced)
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsEnabled
        {
            get
            {
                if (this.BlockID == 0)
                {
                    return false;
                }

                if (this.IsAdvanced && !Options.Get.IsAdvancedModeEnabled)
                {
                    return false;
                }

                if (!Options.Get.EnableStates.ContainsKey(SettingsKey))
                {
                    Options.Get.EnableStates[SettingsKey] = !this.IsAdvanced;
                }

                return Options.Get.EnableStates[SettingsKey];
            }
            set
            {
                Options.Get.EnableStates[SettingsKey] = value;
            }
        }

        public string GetBlockNameAndData(bool isSide)
        {
            return isSide ? this.SideBlockName : this.TopBlockName;
        }

        public Bitmap getImage(bool isSide)
        {
            if (isSide)
            {
                return this.SideImage;
            }
            else
            {
                return this.TopImage;
            }
        }

        public Color getAverageColor(bool isSide)
        {
            if (isSide)
            {
                if (_averageColorSide == null)
                {
                    _averageColorSide = getAverageColor(this.SideImage);
                }
                return _averageColorSide.Value;
            }
            else
            {
                if (_averageColor == null)
                {
                    _averageColor = getAverageColor(this.TopImage);
                }
                return _averageColor.Value;
            }
        }

        private Color getAverageColor(Bitmap src)
        {
            long r = 0;
            long g = 0;
            long b = 0;
            long a = 0;
            long total = src.Width * src.Height;
            if (src.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                src.ToViewStream(null, (int x, int y, Color c) =>
                {
                    r += c.R;
                    g += c.G;
                    b += c.B;
                    a += c.A;
                });
            }
            else
            {
                for (int x = 0; x < src.Width; x++)
                {
                    for (int y = 0; y < src.Height; y++)
                    {
                        Color c = src.GetPixel(x, y);
                        {
                            r += c.R;
                            g += c.G;
                            b += c.B;
                            a += c.A;
                        }
                    }
                }
            }

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            if (a > 128)
            {
                a = 255;
            }

            return Color.FromArgb((int)a, (int)r, (int)g, (int)b).Normalize();
        }

        public override string ToString()
        {
            return this.Label;
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
