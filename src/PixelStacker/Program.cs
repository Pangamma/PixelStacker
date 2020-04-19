using PixelStacker.Logic;
using PixelStacker.Logic.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker
{
    static class Program
    {
        // TODO: Add grasses and dirts
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += MainForm.OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += MainForm.OnUnhandledException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            SetLocaleByTextureSize(Constants.TextureSize);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
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
