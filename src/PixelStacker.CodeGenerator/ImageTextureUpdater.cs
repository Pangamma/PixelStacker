using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PixelStacker.CodeGenerator
{
    [TestClass]
    [TestCategory("Tools")]
    public class ImageTextureUpdater
    {
        private string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.CodeGenerator\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        private string McVersion = "1.18.1";
        private string PxImageDir => Path.Combine(RootDir, "PixelStacker.Resources", "Images", "Textures", "x16");
        private string McImageJar => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            ".minecraft", "versions", McVersion, McVersion + ".jar");


        [TestMethod]
        public void TestZipUpdate()
        {
            var dstDir = new DirectoryInfo(PxImageDir);
            Dictionary<string, string> nameToPaths = new Dictionary<string, string>();
            dstDir.GetFilesRecursively(f => nameToPaths[f.Name] = f.FullName);
            GetJarFiles(McImageJar, (f) => { 
                if (nameToPaths.ContainsKey(f.Name))
                {
                    using (var zipStream = f.Open())
                    {
                        using (var ms = new MemoryStream())
                        {
                            zipStream.CopyTo(ms); // here
                            ms.Position = 0;
                            var arr = ms.ToArray();
                            SKBitmap bm = SKBitmap.Decode(ms);
                            if (bm.Width != 16 || bm.Height != 16)
                            {
                                Debug.WriteLine(f.Name + " is a weird size. Skipping?");
                                return;
                            }

                            File.WriteAllBytes(nameToPaths[f.Name], arr);
                            //canvas.CanvasData = await CanvasData.FromBitmapAsync(canvas.MaterialPalette, bm, worker);
                        }
                    }
                }
            });
        }

        public void GetJarFiles(string filePath, Action<ZipArchiveEntry> actn)
        {
            if (!File.Exists(filePath)) return;

            try
            {

                using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {
                        var entries = archive.Entries;
                        foreach(var entry in entries)
                        {
                            if (!entry.Name.EndsWith(".png")) continue;
                            if (!entry.FullName.Contains("textures")) continue;
                            if (!entry.FullName.Contains("assets")) continue;
                            if (!entry.FullName.Contains("block")) continue;
                            actn.Invoke(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return;
        }

    }
}
