using Newtonsoft.Json;
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

                        {
                            // GRID location
                            int weX = MainForm.Self.LoadedBlueprint?.WorldEditOrigin.X ?? 0;
                            int weY = MainForm.Self.LoadedBlueprint?.WorldEditOrigin.Y ?? 0;
                            var colorMap = Materials.ColorMap.Select(x => $"{x.Key}\t{string.Join("\t", x.Value.Select(m => m.ToString()))}");
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
                // "Failed to save. Error: "
            }
        }

        public static void LoadProject(string filePath)
        {
        }
    }
}
