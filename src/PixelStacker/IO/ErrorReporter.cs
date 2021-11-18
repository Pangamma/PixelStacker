using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.IO
{
    public class ErrorReporter
    {
        public static MainForm MF = null;
        /// <summary>
        /// Triggered when exception occurs off of UI thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ErrorReportInfo info = MainForm.GetErrorReportInfo(MF);
            info.Exception = (Exception)e.ExceptionObject;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ErrorSender()
            {
                CurrentException = (Exception)e.ExceptionObject,
                ReportInfo = info
            });
        }

        /// <summary>
        /// Catches exceptions on the UI thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                ErrorReportInfo info = MainForm.GetErrorReportInfo(MF);
                info.Exception = e.Exception;
                var ui = new ErrorSender()
                {
                    CurrentException = e.Exception,
                    ReportInfo = info
                };

                ui.Show();
            }
        }

        /// <summary>
        /// Returns an exception if one occurs. Otherwise, everything went alright.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="isImageIncluded"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<byte[]> SendExceptionInfoToZipBytes(CancellationToken worker, Exception ex, ErrorReportInfo info, bool isImageIncluded, string userComment)
        {
            try
            {
                using (MemoryStream zipToOpen = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        // Exception info
                        {
                            ZipArchiveEntry entry = archive.CreateEntry("user-comment.txt");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(userComment);
                            }
                        }
                        // System info
                        {
                            var json = JsonConvert.SerializeObject(new Dictionary<string, object>() {
                            { "operating-system", Environment.OSVersion.VersionString },
                            { "machine-name", Environment.MachineName },
                            { "processor-count", Environment.ProcessorCount },
                            { "is-64-bit-process", Environment.Is64BitProcess },
                            { "is-64-bit-os", Environment.Is64BitOperatingSystem },
                            { "physical-memory-mb", Environment.WorkingSet / 1024 / 1024 },
                            { "page-memory-mb", Environment.SystemPageSize / 1024 / 1024 },
                            { "version", Constants.Version },
                        }, Formatting.Indented);

                            ZipArchiveEntry entry = archive.CreateEntry("system-info.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(json);
                            }
                        }

                        // Exception info
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
                                    var aex = (AggregateException)iex;
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

                        // Options
                        if (info?.Options != null)
                        {
                            var optionsJson = JsonConvert.SerializeObject(info.Options, Formatting.Indented);
                            ZipArchiveEntry entry = archive.CreateEntry("options.json");
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                writer.Write(optionsJson);
                            }
                        }

                        // Image content
                        if (isImageIncluded)
                        {
                            if (info.LoadedImage != null)
                            {
                                SKBitmap bm = info.LoadedImage;
                                ZipArchiveEntry entry = archive.CreateEntry("loaded-image.png");
                                using (StreamWriter writer = new StreamWriter(entry.Open()))
                                {
                                    var encoded = bm.Encode(writer.BaseStream, SKEncodedImageFormat.Png, 100);
                                }
                            }
                        }

                        if (info?.RenderedCanvas != null)
                        {
                            var canvas = info.RenderedCanvas;
                            if (canvas.MaterialPalette != null)
                            {
                                var json = JsonConvert.SerializeObject(canvas.MaterialPalette);
                                ZipArchiveEntry entry = archive.CreateEntry("palette.json");
                                using (StreamWriter writer = new StreamWriter(entry.Open()))
                                {
                                    await writer.WriteAsync(json);
                                }
                            }

                            if (canvas.WorldEditOrigin != null)
                            {
                                var json = JsonConvert.SerializeObject(canvas.WorldEditOrigin);
                                ZipArchiveEntry entry = archive.CreateEntry("world-edit-origin.json");
                                using (StreamWriter writer = new StreamWriter(entry.Open()))
                                {
                                    await writer.WriteAsync(json);
                                }
                            }

                            if (isImageIncluded)
                            {
                                if (canvas.CanvasData != null)
                                {
                                    SKBitmap bm = await canvas.CanvasData.ToBitmapAsync(worker);
                                    ZipArchiveEntry entry = archive.CreateEntry("canvas-data.png");
                                    using (StreamWriter writer = new StreamWriter(entry.Open()))
                                    {
                                        var encoded = bm.Encode(writer.BaseStream, SKEncodedImageFormat.Png, 100);
                                    }
                                }

                                if (canvas.PreprocessedImage != null)
                                {
                                    SKBitmap bm = canvas.PreprocessedImage;
                                    ZipArchiveEntry entry = archive.CreateEntry("preprocessed-image.png");
                                    using (StreamWriter writer = new StreamWriter(entry.Open()))
                                    {
                                        var encoded = bm.Encode(writer.BaseStream, SKEncodedImageFormat.Png, 100);
                                    }
                                }
                            }
                        }

                        //if (info?.Options != null) {
                        //    var json = JsonConvert.SerializeObject(MF.PanZoomSettings, Formatting.Indented);
                        //    ZipArchiveEntry entry = archive.CreateEntry("pan-zoom-settings.json");
                        //    using (StreamWriter writer = new StreamWriter(entry.Open()))
                        //    {
                        //        writer.Write(json);
                        //    }
                        //}

                        //if (MainForm.Self?.History != null)
                        //{
                        //    var json = JsonConvert.SerializeObject(MainForm.Self.History, Formatting.Indented);
                        //    ZipArchiveEntry entry = archive.CreateEntry("edit-history.json");
                        //    using (StreamWriter writer = new StreamWriter(entry.Open()))
                        //    {
                        //        writer.Write(json);
                        //    }
                        //}

                        //{
                        //    var colorMap = ColorMatcher.Get.ColorToMaterialMap.Select(x => $"{x.Key.ToArgb()}\t{string.Join("\t", x.Value.Select(m => m.PixelStackerID))}");
                        //    var content = string.Join("\r\n", colorMap);
                        //    ZipArchiveEntry entry = archive.CreateEntry("color-map.dat");
                        //    using (StreamWriter writer = new StreamWriter(entry.Open()))
                        //    {
                        //        writer.Write(content);
                        //    }
                        //}

                    }

                    return zipToOpen.ToArray();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Oh no. An error occurred.");
            }

            return null;
        }
    }
}
