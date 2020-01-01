using ColorMine.ColorSpaces;
using PixelStacker.Logic.Extensions;
using PixelStacker.Properties;
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

        public Material(string category, string pixelStackerID, string label, int blockID, int data, Bitmap topImage, Bitmap sideImage, string topBlockName, string sideBlockName, string schematicaMaterialName)
        {
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
                + ")";
        }

        private string SettingsKey { get { return string.Format("{0}_{1}_{2}", BlockID, Data, Label.Replace(' ', '_').ToLower()); } }

        public bool IsEnabled
        {
            get
            {
                if (!Options.Get.EnableStates.ContainsKey(SettingsKey))
                {
                    Options.Get.EnableStates[SettingsKey] = true;
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
            }else {
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
                src.ToViewStream(null, (int x, int y, Color c) => {
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
