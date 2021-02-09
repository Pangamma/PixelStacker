using fNbt;
using Newtonsoft.Json;
using PixelStacker.Logic.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=netframework-4.8
    /// </summary>
    public class PixelStackerProjectFormatter
    {
        public static void SaveProject(string filePath)
        {
            try
            {
                File.Delete(filePath);

                using (FileStream zipToOpen = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        {
                            ZipArchiveEntry entry = archive.CreateEntry("readme.txt");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.WriteLine("Oh hey! I see you're curious about how this save file works. Let me help you out.");
                                writer.WriteLine("The files contained within this archive serialize the settings and data used at the");
                                writer.WriteLine("time of your last save. You can then re-import or load them again at a later time.");
                                writer.WriteLine("It is recommended that you LOAD save files using the same version of the program that");
                                writer.WriteLine("saved them. Recommended because save formats may change between versions.\r\n");
                                writer.WriteLine("Version @ time of save: " + Constants.Version + "\r\n");
                                writer.WriteLine("If you need a previous version, you can find them in the releases section on github:");
                                writer.WriteLine("https://github.com/Pangamma/PixelStacker/tags");
                            }
                        }

                        {
                            var optionsJson = JsonConvert.SerializeObject(Options.Get);
                            ZipArchiveEntry entry = archive.CreateEntry("options.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(optionsJson);
                            }
                        }

                        {
                            var json = JsonConvert.SerializeObject(MainForm.PanZoomSettings);
                            ZipArchiveEntry entry = archive.CreateEntry("pan-zoom-settings.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(json);
                            }
                        }

                        var matIndex = new Dictionary<string, int>();

                        {
                            int n = 0;
                            matIndex = Materials.List.ToDictionary(m => m.PixelStackerID, m => n++);
                            var json = JsonConvert.SerializeObject(matIndex);
                            ZipArchiveEntry entry = archive.CreateEntry("materials-map-index.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(json);
                            }
                        }

                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (var kvp in ColorMatcher.Get.ColorToMaterialMap)
                            {
                                sb.AppendLine($"{kvp.Key.ToArgb()}\t{string.Join("\t", kvp.Value.Select(v => matIndex[v.PixelStackerID]))}");
                            }

                            ZipArchiveEntry entry = archive.CreateEntry("colors-to-materials-map.txt");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {

                                writer.Write(sb.ToString());
                            }
                        }

                        {
                            // GRID location
                            int weX = MainForm.Self.LoadedBlueprint?.WorldEditOrigin.X ?? 0;
                            int weY = MainForm.Self.LoadedBlueprint?.WorldEditOrigin.Y ?? 0;
                            int[,] blocksMap = MainForm.Self.LoadedBlueprint?.BlocksMap ?? new int[1, 1];

                            ZipArchiveEntry entry = archive.CreateEntry("blueprint.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                //writer.Write(json);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static Exception LoadProject(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Exception("File could not be loaded or it does not exist."); // Failed to load.
            }

            try
            {

                using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {
                        {
                            ZipArchiveEntry entry = archive.GetEntry("options.json");
                            using (StreamReader reader = new StreamReader(entry.Open()))
                            {
                                string json = reader.ReadToEnd();
                                var obj = JsonConvert.DeserializeObject<Options>(json);
                                Options.Load(obj);
                                // TODO: update all UI parts around the app.
                            }
                        }

                        {
                            ZipArchiveEntry entry = archive.GetEntry("pan-zoom-settings.json");
                            using (StreamReader reader = new StreamReader(entry.Open()))
                            {
                                string json = reader.ReadToEnd();
                                var obj = JsonConvert.DeserializeObject<PanZoomSettings>(json);
                                MainForm.PanZoomSettings = obj;
                            }
                        }


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
                return ex;
            }

            return null;
        }
    }
}
