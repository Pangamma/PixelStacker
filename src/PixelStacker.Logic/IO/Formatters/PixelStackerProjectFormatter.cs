using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using SkiaSharp;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public class PixelStackerProjectFormatter : IImportFormatter, IExportFormatter
    {
        private static string Format(string s)
        {
            int lineLen = 88;
            StringBuilder buffer = new StringBuilder();
            string line = "";
            string[] words = s.Replace("\r", "").Replace("\n", "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                if (line.Length + word.Length + 1 <= lineLen)
                {
                    line += " " + word;
                }
                else
                {
                    buffer.AppendLine(line.Trim());
                    line = word;
                }
            }
            buffer.AppendLine(line.Trim());
            string rt = buffer.ToString();
            return rt;
        }

        public bool CanImportFile(string filePath)
            => filePath.EndsWith("pxlzip");

        public async Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            var data = await this.ExportAsync(canvas, worker);
            await File.WriteAllBytesAsync(filePath, data, worker ?? CancellationToken.None);
        }

        public async Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker)
        {
            try
            {
                using (MemoryStream zipToOpen = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        {
                            ZipArchiveEntry entry = archive.CreateEntry("readme.txt");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                await writer.WriteLineAsync(Format(@"Oh hey! I see you're curious about how this save file works. Let me help you out.
                                The files contained within this archive serialize the settings and data used at the time of your last save. You can then
                                re-import or load them again at a later time. It is recommended that you LOAD save files using the same version of the 
                                program that saved them. Recommended because save formats may change between versions."));
                                await writer.WriteLineAsync();
                                await writer.WriteLineAsync("Version @ time of save: " + Constants.Version + "\r\n");
                                await writer.WriteLineAsync("If you need a previous version, you can find them in the releases section on github:");
                                await writer.WriteLineAsync("https://github.com/Pangamma/PixelStacker/tags");
                                await writer.WriteLineAsync();
                                await writer.WriteLineAsync("----------  world-edit-origin.json  ----------------------------------------------------");
                                await writer.WriteLineAsync(Format("Very simple file that remembers where the worldedit origin will be when pasting it as a schematic."));
                                await writer.WriteLineAsync("----------  palette.json  --------------------------------------------------------------");
                                await writer.WriteLineAsync(Format("A mapping that relates material combinations with their respective ID values. I save this along with your project so that it can be imported into future versions of PixelStacker should anything suddenly get added or changed. 'ID: BOTTOM_MATERIAL, TOP_MATERIAL'"));
                                await writer.WriteLineAsync("----------  canvas-data.png  -----------------------------------------------------------");
                                await writer.WriteLineAsync(Format(@"This PNG image contains the data for your rendered project. As
                                you customize it in future steps, this PNG will stay updated with your latest changes. Also, this
                                image probably will not look very good if you try to open it. That is because every pixel's data
                                in the image isn't mapped to a color, but rather to the ID value of the material combination that
                                pixel is supposed to represent. So for AIR, the value would be 0."));
                                await writer.WriteLineAsync("----------  preprocessed-image.png  ----------------------------------------------------");
                                await writer.WriteLineAsync(Format(@"This is your original image after being resized, flattened, 
                                quantized, and dithered. If you ever need to compare the current color with the original image's color
                                this preprocessed image will be there for you. Also the program may decide to use it when recommending
                                similar blocks to you."));
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
                            SKBitmap bm = await canvas.CanvasData.ToBitmapAsync(worker);
                            ZipArchiveEntry entry = archive.CreateEntry("canvas-data.png");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                var encoded = bm.Encode(writer.BaseStream, SKEncodedImageFormat.Png, 100);
                            }
                        }

                        {
                            SKBitmap bm = canvas.PreprocessedImage;
                            ZipArchiveEntry entry = archive.CreateEntry("preprocessed-image.png");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                var encoded = bm.Encode(writer.BaseStream, SKEncodedImageFormat.Png, 100);
                            }
                        }
                    }

                    byte[] data = zipToOpen.ToArray();
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Array.Empty<byte>();
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
                            ZipArchiveEntry entry = archive.GetEntry("world-edit-origin.json");
                            using (StreamReader reader = new StreamReader(entry.Open()))
                            {
                                string json = await reader.ReadToEndAsync();
                                var xy = JsonConvert.DeserializeObject<PxPoint>(json) ?? new PxPoint(0, 0);
                                canvas.WorldEditOrigin = xy;
                            }
                        }

                        {
                            ZipArchiveEntry entry = archive.GetEntry("canvas-data.png");
                            using (var zipStream = entry.Open())
                            {
                                using (var ms = new MemoryStream())
                                {
                                    zipStream.CopyTo(ms); // here
                                    ms.Position = 0;
                                    SKBitmap bm = SKBitmap.Decode(ms);
                                    canvas.CanvasData = await CanvasData.FromBitmapAsync(canvas.MaterialPalette, bm, worker);
                                }
                            }
                        }

                        {
                            ZipArchiveEntry entry = archive.GetEntry("preprocessed-image.png");
                            using (var zipStream = entry.Open())
                            {
                                using (var ms = new MemoryStream())
                                {
                                    zipStream.CopyTo(ms); // here
                                    ms.Position = 0;

                                    SKBitmap bm = SKBitmap.Decode(ms);
                                    canvas.PreprocessedImage = bm;
                                }
                            }
                        }

                        // Make sure to override this.
                        canvas.IsSideView = false;

                        return canvas;
                    }
                }
            }
            catch (Exception ex)
            {
                ProgressX.Report(100, ex.Message);
            }

            return null;
        }
    }
}
