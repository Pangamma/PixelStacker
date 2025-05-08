using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace PixelStacker.CodeGenerator
{
    [TestClass]
    [TestCategory("Tools")]
    public class ImageResxGenerator
    {
        private string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.CodeGenerator\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

        [TestMethod]
        public void ImageResx_Shadows()
        {
            string resxPath = Path.Combine(RootDir, "PixelStacker.Resources", "Shadows.resx");
            SkiaGenerateDesignerFile(resxPath);
            ImageSharpGenerateDesignerFile(resxPath);
            BitmapGenerateDesignerFile(resxPath);
        }

        [TestMethod]
        public void ImageResx_DevResources()
        {
            string resxPath = Path.Combine(RootDir, "PixelStacker.Resources", "DevResources.resx");
            ImageSharpGenerateDesignerFile(resxPath);
            SkiaGenerateDesignerFile(resxPath);
            BitmapGenerateDesignerFile(resxPath);
        }
        
        [TestMethod]
        [Obsolete("This is more dynamic now.")]
        public void ImageResx_Textures()
        {
            string resxPath = Path.Combine(RootDir, "PixelStacker.Resources", "Textures.resx");
            SkiaGenerateDesignerFile(resxPath);
            ImageSharpGenerateDesignerFile(resxPath);
            BitmapGenerateDesignerFile(resxPath);
        }

        private static string FormatPropertyName(string propNm)
        {
            propNm = new string(propNm.Select<char, char>(x => Char.IsLetterOrDigit(x) ? x : '_').ToArray());
            if (Char.IsDigit(propNm[0])) propNm = "_" + propNm;
            return propNm;
        }

        public static void BitmapGenerateDesignerFile(string resxFilePath)
        {
            if (resxFilePath != null) return;
            if (!File.Exists(resxFilePath))
            {
                throw new Exception("File not found. " + resxFilePath);
            }

            Dictionary<string, string> kvps = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(resxFilePath);
            XmlNodeList dataNodes = doc.SelectNodes("//root/data");
            foreach (var kvp in dataNodes.OfType<XmlElement>())
            {
                if (kvp.HasAttribute("name") && kvp.HasChildNodes)
                {
                    string name = kvp.GetAttribute("name");
                    if (name.StartsWith("$")) continue;
                    if (name.StartsWith(">>")) continue;

                    var child = kvp.ChildNodes.OfType<XmlElement>().FirstOrDefault();
                    if (child != null)
                    {
                        string val = child.InnerText.Split(';', StringSplitOptions.RemoveEmptyEntries).First();
                        kvps[name] = val;
                    }
                }
            }

            string clssName = Path.GetFileNameWithoutExtension(resxFilePath);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#if !SKIA_SHARP && !IMAGE_SHARP");
            sb.AppendLine("#pragma warning disable IDE1006 // Naming Styles");
            sb.AppendLine("namespace PixelStacker.Resources {");
            sb.AppendLine("\tusing System;");
            sb.AppendLine("\tusing System.Drawing.Common;");
            sb.AppendLine();
            sb.AppendLine($"\tpublic class {clssName} {{");
            sb.AppendLine($@"
        private static global::System.Resources.ResourceManager resourceMan;
        public static global::System.Resources.ResourceManager ResourceManager {{
            get {{
                if (resourceMan is null) {{
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(""PixelStacker.Resources.{clssName}"", typeof({clssName}).Assembly);
                    resourceMan = temp;
                }}
                return resourceMan;
            }}
        }}");

            foreach (var kvp in kvps)
            {
                sb.AppendLine(@$"
        private static Bitmap _{FormatPropertyName(kvp.Key)} = null;
        public static Bitmap {FormatPropertyName(kvp.Key)} {{
            get {{
                if (_{FormatPropertyName(kvp.Key)} == null)
                    _{FormatPropertyName(kvp.Key)} = SKBitmap.Decode((byte[])ResourceManager.GetObject(""{kvp.Key}""))
                    .Copy(SKColorType.Rgba8888);
                return _{FormatPropertyName(kvp.Key)};
            }}
        }}");
            }

            sb.AppendLine("\t}"); // end class
            sb.AppendLine("}"); // end namespace
            sb.AppendLine("#pragma warning restore IDE1006 // Naming Styles");
            sb.AppendLine("#endif");


            string rs = sb.ToString();
            string toSavePath = resxFilePath.Replace(".resx", ".Bitmap.cs");
            File.WriteAllText(toSavePath, rs);
        }

        public static void SkiaGenerateDesignerFile(string resxFilePath)
        {
            if (!File.Exists(resxFilePath))
            {
                throw new Exception("File not found. " + resxFilePath);
            }

            Dictionary<string, string> kvps = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(resxFilePath);
            XmlNodeList dataNodes = doc.SelectNodes("//root/data");
            foreach (var kvp in dataNodes.OfType<XmlElement>())
            {
                if (kvp.HasAttribute("name") && kvp.HasChildNodes)
                {
                    string name = kvp.GetAttribute("name");
                    if (name.StartsWith("$")) continue;
                    if (name.StartsWith(">>")) continue;

                    var child = kvp.ChildNodes.OfType<XmlElement>().FirstOrDefault();
                    if (child != null)
                    {
                        string val = child.InnerText.Split(';', StringSplitOptions.RemoveEmptyEntries).First();
                        kvps[name] = val;
                    }
                }
            }

            string clssName = Path.GetFileNameWithoutExtension(resxFilePath);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#if SKIA_SHARP");
            sb.AppendLine("#pragma warning disable IDE1006 // Naming Styles");
            sb.AppendLine("namespace PixelStacker.Resources {");
            sb.AppendLine("\tusing System;");
            sb.AppendLine("\tusing SkiaSharp;");
            sb.AppendLine();
            sb.AppendLine($"\tpublic class {clssName} {{");
            sb.AppendLine($@"
        private static global::System.Resources.ResourceManager resourceMan;
        public static global::System.Resources.ResourceManager ResourceManager {{
            get {{
                if (resourceMan is null) {{
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(""PixelStacker.Resources.{clssName}"", typeof({clssName}).Assembly);
                    resourceMan = temp;
                }}
                return resourceMan;
            }}
        }}");

            foreach(var kvp in kvps)
            {
                sb.AppendLine(@$"
        private static SKBitmap _{FormatPropertyName(kvp.Key)} = null;
        public static SKBitmap {FormatPropertyName(kvp.Key)} {{
            get {{
                if (_{FormatPropertyName(kvp.Key)} == null)
                    _{FormatPropertyName(kvp.Key)} = SKBitmap.Decode((byte[])ResourceManager.GetObject(""{kvp.Key}""))
                    .Copy(SKColorType.Rgba8888);
                return _{FormatPropertyName(kvp.Key)};
            }}
        }}");
            }

            sb.AppendLine("\t}"); // end class
            sb.AppendLine("}"); // end namespace
            sb.AppendLine("#pragma warning restore IDE1006 // Naming Styles");
            sb.AppendLine("#endif");


            string rs = sb.ToString();
            string toSavePath = resxFilePath.Replace(".resx", ".Skia.cs");
            File.WriteAllText(toSavePath, rs);
        }

        public static void ImageSharpGenerateDesignerFile(string resxFilePath)
        {
            if (!File.Exists(resxFilePath))
            {
                throw new Exception("File not found. " + resxFilePath);
            }

            Dictionary<string, string> kvps = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(resxFilePath);
            XmlNodeList dataNodes = doc.SelectNodes("//root/data");
            foreach (var kvp in dataNodes.OfType<XmlElement>())
            {
                if (kvp.HasAttribute("name") && kvp.HasChildNodes)
                {
                    string name = kvp.GetAttribute("name");
                    if (name.StartsWith("$")) continue;
                    if (name.StartsWith(">>")) continue;

                    var child = kvp.ChildNodes.OfType<XmlElement>().FirstOrDefault();
                    if (child != null)
                    {
                        string val = child.InnerText.Split(';', StringSplitOptions.RemoveEmptyEntries).First();
                        kvps[name] = val;
                    }
                }
            }

            string clssName = Path.GetFileNameWithoutExtension(resxFilePath);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#if IMAGE_SHARP");
            sb.AppendLine("#pragma warning disable IDE1006 // Naming Styles");
            sb.AppendLine("namespace PixelStacker.Resources {");
            sb.AppendLine("\tusing System;");
            sb.AppendLine("\tusing SixLabors.ImageSharp;");
            sb.AppendLine("\tusing SixLabors.ImageSharp.PixelFormats;");
            sb.AppendLine();
            sb.AppendLine($"\tpublic class {clssName} {{");
            sb.AppendLine($@"
        private static global::System.Resources.ResourceManager resourceMan;
        public static global::System.Resources.ResourceManager ResourceManager {{
            get {{
                if (resourceMan is null) {{
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(""PixelStacker.Resources.{clssName}"", typeof({clssName}).Assembly);
                    resourceMan = temp;
                }}
                return resourceMan;
            }}
        }}");

            foreach (var kvp in kvps)
            {
                sb.AppendLine(@$"
        private static Image<Rgba32> _{FormatPropertyName(kvp.Key)} = null;
        public static Image<Rgba32> {FormatPropertyName(kvp.Key)} {{
            get {{
                if (_{FormatPropertyName(kvp.Key)} == null)
                    _{FormatPropertyName(kvp.Key)} = Image.Load((byte[])ResourceManager.GetObject(""{kvp.Key}""));
                return _{FormatPropertyName(kvp.Key)};
            }}
        }}");
            }

            sb.AppendLine("\t}"); // end class
            sb.AppendLine("}"); // end namespace
            sb.AppendLine("#pragma warning restore IDE1006 // Naming Styles");
            sb.AppendLine("#endif");


            string rs = sb.ToString();
            string toSavePath = resxFilePath.Replace(".resx", ".ImageSharp.cs");
            File.WriteAllText(toSavePath, rs);
        }
}
}
