using Newtonsoft.Json;
using PixelStacker.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.IO.Formatters
{
    public class PixelStackerProjectFormatter : IImportFormatter, IExportFormatter
    {
        public async Task ExportAsync(string filePath, RenderedCanvas canvas, CancellationToken? worker)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                using (FileStream zipToOpen = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        {
                            ZipArchiveEntry entry = archive.CreateEntry("readme.txt");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                await writer.WriteLineAsync("Oh hey! I see you're curious about how this save file works. Let me help you out.");
                                await writer.WriteLineAsync("The files contained within this archive serialize the settings and data used at the");
                                await writer.WriteLineAsync("time of your last save. You can then re-import or load them again at a later time.");
                                await writer.WriteLineAsync("It is recommended that you LOAD save files using the same version of the program that");
                                await writer.WriteLineAsync("saved them. Recommended because save formats may change between versions.\r\n");
                                await writer.WriteLineAsync("Version @ time of save: " + Constants.Version + "\r\n");
                                await writer.WriteLineAsync("If you need a previous version, you can find them in the releases section on github:");
                                await writer.WriteLineAsync("https://github.com/Pangamma/PixelStacker/tags");
                            }
                        }

                        {
                            var json = JsonConvert.SerializeObject(canvas.MaterialPalette);
                            ZipArchiveEntry entry = archive.CreateEntry("palette.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                await writer.WriteAsync(json);
                            }
                        }

                        {
                            var json = JsonConvert.SerializeObject(canvas.WorldEditOrigin);
                            ZipArchiveEntry entry = archive.CreateEntry("world-edit-origin.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                await writer.WriteAsync(json);
                            }
                        }

                        {
                            Bitmap bm = await canvas.CanvasData.ToBitmapAsync(worker);
                            ZipArchiveEntry entry = archive.CreateEntry("canvas-data.png");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                bm.Save(writer.BaseStream, ImageFormat.Png);
                            }
                        }

                        {
                            Bitmap bm = canvas.OriginalImage;
                            ZipArchiveEntry entry = archive.CreateEntry("original-image.png");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                bm.Save(writer.BaseStream, ImageFormat.Png);
                            }
                        }


                        //{
                        //    var json = JsonConvert.SerializeObject(MainForm.PanZoomSettings);
                        //    ZipArchiveEntry entry = archive.CreateEntry("pan-zoom-settings.json");
                        //    using (StreamWriter writer = new StreamWriter(entry.Open()))
                        //    {
                        //        writer.Write(json);
                        //    }
                        //}

                        //var matIndex = new Dictionary<string, int>();

                        //{
                        //    int n = 0;
                        //    matIndex = Materials.List.ToDictionary(m => m.PixelStackerID, m => n++);
                        //    var json = JsonConvert.SerializeObject(matIndex);
                        //    ZipArchiveEntry entry = archive.CreateEntry("materials-map-index.json");
                        //    using (StreamWriter writer = new StreamWriter(entry.Open()))
                        //    {
                        //        writer.Write(json);
                        //    }
                        //}

                        //{
                        //    StringBuilder sb = new StringBuilder();
                        //    foreach (var kvp in ColorMatcher.Get.ColorToMaterialMap)
                        //    {
                        //        sb.AppendLine($"{kvp.Key.ToArgb()}\t{string.Join("\t", kvp.Value.Select(v => matIndex[v.PixelStackerID]))}");
                        //    }

                        //    ZipArchiveEntry entry = archive.CreateEntry("colors-to-materials-map.txt");
                        //    using (StreamWriter writer = new StreamWriter(entry.Open()))
                        //    {

                        //        writer.Write(sb.ToString());
                        //    }
                        //}

                        //{
                        //    // GRID location
                        //    int weX = MainForm.Self.LoadedBlueprint?.WorldEditOrigin.X ?? 0;
                        //    int weY = MainForm.Self.LoadedBlueprint?.WorldEditOrigin.Y ?? 0;
                        //    int[,] blocksMap = MainForm.Self.LoadedBlueprint?.BlocksMap ?? new int[1, 1];

                        //    ZipArchiveEntry entry = archive.CreateEntry("blueprint.json");
                        //    using (StreamWriter writer = new StreamWriter(entry.Open()))
                        //    {
                        //        //writer.Write(json);
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<RenderedCanvas> ImportAsync(string filePath, CancellationToken? worker = null)
        {
            if (!File.Exists(filePath)) return null;

            try
            {

                using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {
                        var canvas = new RenderedCanvas();

                        {
                            ZipArchiveEntry entry = archive.GetEntry("palette.json");
                            using (StreamReader reader = new StreamReader(entry.Open()))
                            {
                                string json = await reader.ReadToEndAsync();
                                var obj = JsonConvert.DeserializeObject<MaterialPalette>(json);
                                canvas.MaterialPalette = obj;
                                if (obj == null)
                                {
                                    ProgressX.Report(100, "Failed to load palette.json from file.");
                                    return null;
                                }
                            }
                        }


                        {
                            ZipArchiveEntry entry = archive.GetEntry("canvas-data.png");
                            using (StreamReader reader = new StreamReader(entry.Open()))
                            {
                                var img = Bitmap.FromStream(reader.BaseStream);
                                Bitmap bm = new Bitmap(img);
                                canvas.CanvasData = await CanvasData.FromBitmapAsync(canvas.MaterialPalette, bm, worker);
                            }
                        }

                        {
                            ZipArchiveEntry entry = archive.GetEntry("original-image.png");
                            using (StreamReader reader = new StreamReader(entry.Open()))
                            {
                                var img = Bitmap.FromStream(reader.BaseStream);
                                Bitmap bm = new Bitmap(img);
                                canvas.OriginalImage = bm;
                            }
                        }

                        {
                            ZipArchiveEntry entry = archive.GetEntry("world-edit-origin.json");
                            using (StreamReader reader = new StreamReader(entry.Open()))
                            {
                                string json = await reader.ReadToEndAsync();
                                canvas.WorldEditOrigin = JsonConvert.DeserializeObject<Point>(json);
                            }
                        }


                        return canvas;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ProgressX.Report(100, ex.Message);
            }

            return null;
        }
    }
}
