using PixelStacker.Logic;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PixelStacker
{
    static class Program
    {
        // TODO: Add grasses and dirts
        // TODO: CalculateTextureSize should be a stand-alone method which takes in:
        // - Original image dimensions
        // - Calculated output image dimensions
        // TODO: PanZoomSettings generator based on above inputs ^
        // 
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Application.ThreadException += MainForm.OnThreadException;
                AppDomain.CurrentDomain.UnhandledException += MainForm.OnUnhandledException;
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                SetLocaleByTextureSize(Constants.TextureSize);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                return;
            } else {
                Bitmap inputImage = null;
                string outputPath = null;
                string format = null;
                foreach (var arg in args)
                {
                    string lArg = arg.ToLowerInvariant();
                    if (lArg.StartsWith("--options="))
                    {
                        Options.Import(arg.Substring("--options=".Length));
                    }
                    else if (lArg.StartsWith("--input="))
                    {
                        inputImage = (Bitmap) Bitmap.FromFile(arg.Substring("--input=".Length));
                    }
                    else if (lArg.StartsWith("--output="))
                    {
                        outputPath = arg.Substring("--output=".Length);
                        string ext = arg.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                        if (ext != null && format == null)
                        {
                            switch (ext.ToUpperInvariant())
                            {
                                case "SCHEMATIC":
                                case "PNG":
                                case "SCHEM": 
                                    format = ext.ToUpperInvariant();
                                    break;
                            }
                        }
                    }
                    else if (lArg.StartsWith("--format="))
                    {
                        format = arg.Substring("--format=".Length).ToUpperInvariant();
                    }
                    else
                    {
                        if (lArg.StartsWith("--help"))
                        {
                            Console.WriteLine(@"
------------------  PixelStacker Help  ----------------------------------------
  The following options are available for CLI usage. If no CLI params are
  provided, the standard program GUI will be used instead.

  --format          Specify the output format to use. Available values are:
                    SCHEM, PNG
                    Example: '--format=PNG'

  --output          Where to output the file. Bitmap.Save({your value})
                    Example: '--output=C:\git\my-output-file.png'

  --input           Where to load the file. Bitmap.FromFile({your value})
                    Example: '--input=C:\git\myfile.png'

  --options         Path to the options.json file to use. If not provided,
                    your most recently used settings will be applied.
                    Example: '--options=C:\git\myfile.json'
");
                            return;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(format))
                {
                    Console.Error.WriteLine($"No value given for the --format argument. Valid values: [PNG, SCHEM]");
                    return;
                }

                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    Console.Error.WriteLine($"No value given for the --output argument. Must be a full file path.");
                    return;
                }
                else if (File.Exists(outputPath))
                {
                    Console.Error.WriteLine($"Warning, the output file already exists. It will be erased.");
                    File.Delete(outputPath);
                }

                if (inputImage == null)
                {
                    Console.Error.WriteLine($"File path for --input was invalid. File not found, or file could not be loaded.");
                    return;
                }

                switch (format)
                {
                    case "PNG":
                        PngFormatter.writePNG(CancellationToken.None, outputPath, inputImage).GetAwaiter().GetResult();
                        Console.WriteLine("Finished!");
                        break;
                    case "SCHEM":
                        Schem2Formatter.writeSchemFromImage(CancellationToken.None, outputPath, inputImage).GetAwaiter().GetResult();
                        Console.WriteLine("Finished!");
                        break;
                }
            }
        }

        /// <summary>
        /// I am not aware of any way to select RESX files based on parameters other than cultureInfo, so
        /// for now I will have to use this method to select the textures set.
        /// </summary>
        /// <param name="textureSize"></param>
        private static void SetLocaleByTextureSize(int textureSize)
        {
            switch (textureSize)
            {
                case 16:
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-us");
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-us");
                    break;
                case 32:
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ja-jp");
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ja-jp");
                    break;
                case 64:
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ko-kr");
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ko-kr");
                    break;
                default:
                    throw new NotImplementedException("Only 16, 32, and 64 are supported.");
            }
        }
    }
}
