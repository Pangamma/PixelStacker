using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace PixelStacker.CodeGenerator
{
    [TestClass]
    [TestCategory("Tools")]
    public class FontIconsRenamer
    {
        private readonly string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.CodeGenerator\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

        [TestMethod]
        public void Find_Text()
        {
            string rootPath = Path.Combine(RootDir, "PixelStacker", "Properties", "Images", "ToolIcons");

            ReadFilesRecursively(new DirectoryInfo(rootPath), (fi) =>
            {
                try
                {
                    if (fi.Extension != ".png") return false;
                    string[] parts = fi.Name.Split('-', StringSplitOptions.RemoveEmptyEntries);
                    if (int.TryParse(parts[0], out int _))
                    {
                        string nm = string.Join("_", parts.Skip(1));
                        fi.MoveTo(Path.Combine(fi.DirectoryName, nm));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                return false;
            });
        }

        private void ReadFilesRecursively(DirectoryInfo dir, Func<FileInfo, bool> fileAction)
        {
            foreach (var file in dir.GetFiles())
            {
                if (fileAction(file))
                {
                    return;
                }
            }

            foreach (var subdir in dir.GetDirectories())
            {
                ReadFilesRecursively(subdir, fileAction);
            }
        }

        private static string FormatPropertyName(string propNm)
        {
            propNm = new string(propNm.Select<char, char>(x => Char.IsLetterOrDigit(x) ? x : '_').ToArray());
            if (Char.IsDigit(propNm[0])) propNm = "_" + propNm;
            return propNm;
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

            foreach (var kvp in kvps)
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
    }
}
