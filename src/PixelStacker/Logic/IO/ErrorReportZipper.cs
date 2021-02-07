using Newtonsoft.Json;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
    public class ErrorReportZipper
    {

        /// <summary>
        /// Returns an exception if one occurs. Otherwise, everything went alright.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="isImageIncluded"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Exception SaveError(Exception ex, bool isImageIncluded, string filePath = "pixelstacker-error-report.zip")
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
                            var json = JsonConvert.SerializeObject(new Dictionary<string, object>() {
                                { "operating-system", Environment.OSVersion.VersionString },
                                { "machine-name", Environment.MachineName },
                                { "version", Constants.Version },
                            }, Formatting.Indented);

                            ZipArchiveEntry entry = archive.CreateEntry("system-info.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(json);
                            }
                        }

                        {
                            var json = JsonConvert.SerializeObject(ex, Formatting.Indented);
                            ZipArchiveEntry entry = archive.CreateEntry("exception.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(json);
                            }
                        }

                        {
                            string text = "";
                            var iex = ex;
                            if (iex != null)
                            {
                                if (iex is AggregateException)
                                {
                                    text += "\r\n\r\n\r\n=AGGREGATE=========================================================\r\n";
                                    text += "WARNING: Will fail to grab inner inner exceptions for the items below. See JSON for more info.\r\n";
                                    var aex = (AggregateException) iex;
                                    foreach (var iiex in aex.InnerExceptions)
                                    {
                                        text += iiex.Message + "\r\n";
                                        text += iiex.StackTrace + "\r\n";
                                        // Will fail to grab inner exceptions of inner exceptions... :C
                                    }
                                }
                                else
                                {
                                    while (iex != null)
                                    {
                                        text += "\r\n\r\n\r\n===============================================================\r\n";
                                        text += iex.Message + "\r\n";
                                        text += iex.StackTrace + "\r\n";
                                        iex = iex.InnerException;
                                    }
                                }
                            }

                            ZipArchiveEntry entry = archive.CreateEntry("exception.txt");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(text);
                            }
                        }

                        {
                            var optionsJson = JsonConvert.SerializeObject(Options.Get, Formatting.Indented);
                            ZipArchiveEntry entry = archive.CreateEntry("options.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(optionsJson);
                            }
                        }

                        {
                            var json = JsonConvert.SerializeObject(MainForm.PanZoomSettings, Formatting.Indented);
                            ZipArchiveEntry entry = archive.CreateEntry("pan-zoom-settings.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(json);
                            }
                        }

                        if (MainForm.Self?.History != null)
                        {
                            var json = JsonConvert.SerializeObject(MainForm.Self.History, Formatting.Indented);
                            ZipArchiveEntry entry = archive.CreateEntry("edit-history.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(json);
                            }
                        }

                        {
                            var colorMap = ColorMatcher.Get.ColorToMaterialMap.Select(x => $"{x.Key.ToArgb()}\t{string.Join("\t", x.Value.Select(m => m.PixelStackerID))}");
                            var content = string.Join("\r\n", colorMap);
                            ZipArchiveEntry entry = archive.CreateEntry("color-map.dat");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(content);
                            }
                        }

                        if (isImageIncluded)
                        {
                            if (MainForm.Self.LoadedImage != null)
                            {
                                using (var img = MainForm.Self.LoadedImage.To32bppBitmap())
                                {
                                    ZipArchiveEntry entry = archive.CreateEntry("loaded-image.png");
                                    using (var stream = new MemoryStream())
                                    {
                                        img.Save(stream, ImageFormat.Png);
                                        using (var entryStream = entry.Open())
                                        {
                                            var bytes = stream.ToArray();
                                            entryStream.Write(bytes, 0, bytes.Length);
                                        }
                                    }
                                }
                            }

                            if (MainForm.Self.PreRenderedImage != null)
                            {
                                using (var img = MainForm.Self.PreRenderedImage.To32bppBitmap())
                                {
                                    ZipArchiveEntry entry = archive.CreateEntry("pre-rendered-image.png");
                                    using (var stream = new MemoryStream())
                                    {
                                        img.Save(stream, ImageFormat.Png);
                                        using (var entryStream = entry.Open())
                                        {
                                            var bytes = stream.ToArray();
                                            entryStream.Write(bytes, 0, bytes.Length);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                return ex2;
                Console.WriteLine("Oh no. An error occurred.");
            }

            return null;
        }
    }
}
